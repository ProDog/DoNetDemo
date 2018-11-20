# NEO 的跨链通讯
## 介绍
### 从通信本质出发
通信是什么？无非就是信息的传递。一般是一方主动发送信息，另一方接收信息；一方暴露出信息，一方主动获取（监听）信息。
基于通信的本质，NEO 的跨链通讯可以使用智能合约来实现，大体思路是用智能合约实现暴露信息的接口和接收信息的接口，由于 NEO 不会主动向外发送信息，所以需要一种方式使得 NEO 能产生出信息，外部来主动获取，产生信息可以使用 NEO 的 log 机制、也就是 notifications（Notify）来实现。

### notifications（Notify）
区块链上记录的只有交易、智能合约的部署和调用也是通过发送交易来实现，所以要通过 NEO 区块链得到交易以外的信息，就需要用到一些附加操作。比如智能合约的运行可以产生 log，即 notifications，也叫作 notify，nep5 类型资产转账交易都会默认产生 notifications，因为 nep5 中的 token 资产转账方法 transfer 默认实现了产生 notifications 的方法。除了 nep5 资产转账以外，其他调用合约的时候也可以产生 notifications，只需要在智能合约中增加产生 notifications 的方法即可。
### 获取 notifications

知道了可以产生 notifications（Notify），那么 notifications 保存在哪里呢？如何获取到它呢？
首先，notifications 不保存在区块链上，它只有在智能合约执行到产生 notifications 的语句时会输出，在使用 cli 节点同步区块数据时会将所有智能合约相关的操作在智能合约虚拟机 neo-vm 中运行一次，合约的状态等信息就是在运行过程中推出来的。在启动 cli 节点时，如果打开了 --log 命令，节点在运行每个区块中的智能合约相关操作时，会将输出的 notifications 保存到本地 nel-cli 目录下的 ApplicationLogs_ 文件夹中，记录为 txid.json 的文件(2.9 以后改为 区块高度.ldb 的文件了，如 003898.ldb)。然后我们使用 cli 提供的 rpc 接口 getapplicationlog 就可以查到对应的 notifications，例如 url?jsonrpc=2.0&id=1&method=getapplicationlog&params=["0xe19ecff18894315e088402fe0d53f7a23ff9f2a54dc1042dc9f31212f4b6764a"]，返回结果：
```
{
    "jsonrpc": "2.0",
    "id": 1,
    "result": {
        "txid": "0x350f91ffc04cca4c8c6ff26f0699e8dce0b397d2abd674ed686c041e2e88504e",
        "vmstate": "HALT, BREAK",
        "gas_consumed": "1.539",
        "stack": [
            {
                "type": "Integer",
                "value": "1"
            }
        ],
        "notifications": [
            {
                "contract": "0x24192c2a72e0ce8d069232f345aea4db032faf72",
                "state": {
                    "type": "Array",
                    "value": [
                        {
                            "type": "ByteArray",
                            "value": "6f757463616c6c"
                        },
                        {
                            "type": "ByteArray",
                            "value": "63616c6c"
                        },
                        {
                            "type": "ByteArray",
                            "value": "f25d29b0059a3feecd3862fea44fdb4351a67755"
                        }
                    ]
                }
            }
        ]
    }
}
```
notifications 是智能合约运行的 log，contract 是智能合约 hash；
state 是输出的数据，value 中是合约中设定的要输出的内容。
### 使用 notifications
使用 notifications 可以实现 nep5 资产的充值监控，具体如下：
1. 通过 getblock api 获取每个区块的详情，其中包括该区块中所有交易的详情；
2. 分析每笔交易的交易类型，过滤出所有类型为 "InvocationTransaction" 的交易，因为 Nep5 资产转账类型均为 "InvocationTransaction";
3. 取到每笔 Nep5 资产交易的 txid，然后调用 getapplicationlog api 获取每笔交易的详情，分析交易进行地址的比对就可判断是否有地址发生交易。

同样的方式，我们也可以使用 notifications 实现跨链通信。

## 设计

* 实现原理：
 NEO 的 nep5 转账交易可以产生 notify，ZoroChain 可以通过观察 notify 得到 NEO 链上产生的信息；
 ZoroChain 也可以通过构造 NEO 链上的交易将信息带到 NEO 链上。
* 实现机制：
     1、在 NEO 上发布支持输出 notify 的智能合约，合约包含接收外部建立通信请求的 call 方法，支持外部传入返回值的 returnvalue 方法等，为了更加安全、需要支持验证签名；
     2、发送一笔调用 call 方法的交易，此时可以设定返回 returnvalue 的见证人，传入一些请求参数、数据等；     
     3、ZoroChain 根据调用 call 的交易 id 进行监控，获取到 notify，从而得到 NEO 链上 call 合约输出的信息；
     4、处理完 notify，ZoroChain 向 Neo 链发送具备专用私钥签名的交易，用于向 Neo 链发送 return value 数据。
     
* 实现过程：
    1、neo 端交易输出 notify 携带信息，该交易称为 A。

    2、ZoroChain 捕获到这个信息，在 rootchain 上发起一笔对应的交易，用 A 作为该交易的 ID。
足够的记账节点捕获到该信息，该信息才能上链（这需要额外的机制，不在此处赘述）。

    3、该交易执行完毕之后，ZoroChain 向 NEO 对应的智能合约发起一笔交易，通知这次调用的返回结果，该交易一样留下独特的 Notify 记号。交易在记账节点执行，每个记账节点都会尝试向 NEO 发起这笔交易（是同一笔交易）。

ZoroChain 捕获到交易 A 对应的 返回结果 notify 后，在 rootchain 上发起一笔对应的调用完成的交易，这个过程可以记为 Rpccall_zoro(txid,state,“xxx”,[]);

对于 NEO 端来说，他会看到两笔 invoke 交易，一笔用于向 zorochain 传递参数（zoro 调用 call 的交易），一笔得到 zorochain 的执行结果（zoro 返回 return value 的交易）。

对于 ZoroChain 端来说，他也会看到两笔交易，一笔传递参数（获得 call 的 notify 后执行处理的交易），一笔显示结果（获得 return value 的 notify 后记录通信完成的交易）

## 实现
### Demo 说明
#### 外部监测
* 从指定高度开始、读取每个区块，使用 getblock 接口；
* 获取区块中每个 txid 的 notifications，使用 getapplicationlog 接口，没有 notifications 的忽略；
* 解析每条 notifications，只关注 contract 为目标合约 hash 的 notifications；
* 得到目标合约的 notify 的 type 和 value。

#### 外部 call
* 建立通信：发起调用目标合约的一笔交易并签名；调用合约 outcall 方法、交易执行后 callstate=1；
* 根据上一步的 txid、用合约的 getcallstate 方法检查 callstate；这一步用 invokescript；


#### 图示

![](跨链.png)