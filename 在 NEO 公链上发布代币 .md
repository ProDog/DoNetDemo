# 在 NEO 公链上发布代币
NEO 系统中有 UTXO 模型的代币 NEO 和 GAS，但如果基于 NEO 开发 Dapp 或其他区块链项目时我们仍然需要能实现自己的代币功能，这里就来总结一下如何用智能合约在 NEO 上发布代币。
与比特币的 BIP、以太坊的 ERC 等类似，NEO 也有 NEPs: NEO Enhancement Proposals  NEO加强/改进提议 ，它描述的是 NEO 平台的标准，包括核心协议规范，客户端 API 和合约标准。
在 NEO 公链上发布代币是 Nep5 即 NEO 5 号改进提案中提出的，所以我们发布的代币统称 Nep5 代币，在 Nep5 中，规定了代币合约必须实现的接口、返回结果等标准，所以我们只需按照标准就可以开发自己的代币，然后发布合约即可，下面就分别讲讲合约编程和发布。
合约的基本介绍和开发前准备可以参考 NEO 开发文档中的智能合约部分，如果想更全面的学习智能合约开发可以看这里相关资料。

nep5：
概述：
nep5提案描述了neo区块链的token标准，它为token类的智能合约提供了系统的通用交互机制，定义了这种机制和每种特性，并提供了开发模板和示例。
动机：随着neo区块链生态的发展，智能合约的部署和调用变得越来越重要，如果没有标准的交互方法，系统就需要为每个智能合约维护一套单独的api，无论合约间有没有相似性。
token类合约的操作机制其实基本都是相同的，因此需要这样一套标准。这些与token交互的标准方案使整个生态系统免于维护每个使用token的智能合约的pai。
规范：
在下面的方法中，我们提供了在合约中函数的定义方式及参数调用。
方法：
totalSupply
public static BigInteger totalSupply（）
返回token总量
name
public static string name()
返回token名称
每次调用时此方法必须返回相同的值
symbol
public static string symbol()
返回此合约中管理的token的简称。3-8字符、限制为大写英文字母
每次调用时此方法必须返回相同的值
decimals
public static byte decimals()
返回token使用的小数位数
每次调用时此方法必须返回相同的值
balanceOf
public static BigInteger balanceOf(byte[] account)
返回token余额
参数account应该是一个20字节的地址。如果没有，这种方法应该throw是一个异常。
如果account是未使用的地址，则此方法必须返回0。
transfer
public static bool transfer(byte[] from, byte[] to, BigInteger amount)
将amount数量的token从from账户转到to账户
参数from和to应该是20字节的地址。如果没有，方法应该throw是一个异常。
参数amount必须大于或等于0。如果没有，方法应该throw是一个异常。
如果from帐户余额没有足够的token可用，则该函数必须返回false。
如果该方法成功，它必须触发transfer事件，并且必须返回true，即使amount是0，或from与to有相同的地址。
函数应该检查from地址是否等于合约调用者hash。如果是这样，应该处理转账; 如果没有，该函数应该使用SYSCALL Neo.Runtime.CheckWitness来验证交易。
如果to地址是已部署的合约地址，则该函数应该检查该合约的payable标志以决定是否应该将token转移到该合约地址。
如果未处理转账，则函数应该返回false。

交易
transfer
public static event transfer(byte[] from, byte[] to, BigInteger amount)
转移token时必须触发，包括零值转移。
创建新token的合约必须触发一个transfer事件，其from地址设置为null时创建token。
燃烧token的合约必须触发一个transfer事件，其to地址设置为null时燃烧token。

实现：
Woolong：https：//github.com/lllwvlvwlll/Woolong
ICO模板：https：//github.com/neo-project/examples/tree/master/ICO_Template

```
using Neo.SmartContract.Framework;
using Neo.SmartContract.Framework.Services.Neo;
using System;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Linq.Expressions;
using System.Numerics;
using System.Runtime.Remoting.Messaging;
using Neo.SmartContract.Framework.Services.System;
using Helper = Neo.SmartContract.Framework.Helper;

namespace ABCContract
{
    public class ABC : SmartContract
    {
        public delegate void deleTransfer(byte[] from, byte[] to, BigInteger value);
        [DisplayName("transfer")]
        public static event deleTransfer Transferred;

        public class TransferInfo
        {
            public byte[] from;
            public byte[] to;
            public BigInteger value;
        }

		//管理员账户，改成自己测试用的的
        private static readonly byte[] superAdmin = Helper.ToScriptHash("AcQLYjGbQU2bEQ8RKFXUcf8XvromfUQodq");

        public static string name()
        {
            return "ABC Coin";//名称
        }

        public static string symbol()
        {
            return "ABC";//简称
        }

        private const ulong factor = 100000000;//精度
        private const ulong totalCoin =  100000000 * factor;//总量 要乘以精度，NEO 系统中没有小数，所有数字类型都转为 BigInteger 处理

        public static byte decimals()
        {
            return 8;
        }

        public static object Main(string method, object[] args)
        {
            var magicstr = "abc-test";
            if (Runtime.Trigger == TriggerType.Verification)
            {
                return false;
            }
            else if (Runtime.Trigger == TriggerType.VerificationR)
            {
                return true;
            }
            else if (Runtime.Trigger == TriggerType.Application)
            {
                //开始时取到调用该合约的脚本hash
                var callscript = ExecutionEngine.CallingScriptHash;

                if (method == "totalSupply")
                    return totalSupply();
                if (method == "name")
                    return name();
                if (method == "symbol")
                    return symbol();
                if (method == "decimals")
                    return decimals();
                //发行，合约发布后由管理员发行代币
                if (method == "deploy")
                {
                    if (args.Length != 1)
                        return false;
                    if (!Runtime.CheckWitness(superAdmin))
                        return false;
                    byte[] total_supply = Storage.Get(Storage.CurrentContext, "totalSupply");
                    if (total_supply.Length != 0)
                        return false;
                    var keySuperAdmin = new byte[] {0x11}.Concat(superAdmin);
                    Storage.Put(Storage.CurrentContext, keySuperAdmin, totalCoin);
                    Storage.Put(Storage.CurrentContext, "totalSupply", totalCoin);

                    Transferred(null, superAdmin, totalCoin);
                }

                //获取余额
                if (method == "balanceOf")
                {
                    if (args.Length != 1)
                        return 0;
                    byte[] who = (byte[]) args[0];
                    if (who.Length != 20)
                        return false;
                    return balanceOf(who);
                }

                //转账接口
                if (method == "transfer")
                {
                    if (args.Length != 3)
                        return false;
                    byte[] from = (byte[]) args[0];
                    byte[] to = (byte[]) args[1];
                    if (from == to)
                        return true;
                    if (from.Length != 20 || to.Length != 20)
                        return false;
                    BigInteger value = (BigInteger) args[2];
                    if (!Runtime.CheckWitness(from))
                        return false;
                        //禁止跳板调用
                    if (ExecutionEngine.EntryScriptHash.AsBigInteger() != callscript.AsBigInteger())
                        return false;
                    if (!IsPayable(to))
                        return false;
                    return transfer(from, to, value);
                }

                //合约脚本的转账接口、弥补没有跳板调用
                if (method == "transfer_app")
                {
                    if (args.Length != 3)
                        return false;
                    byte[] from = (byte[]) args[0];
                    byte[] to = (byte[]) args[1];
                    BigInteger value = (BigInteger) args[2];

                    if (from.AsBigInteger() != callscript.AsBigInteger())
                        return false;
                    return transfer(from, to, value);
                }

                //获取交易信息
                if (method == "getTxInfo")
                {
                    if (args.Length != 1)
                        return 0;
                    byte[] txid = (byte[]) args[0];
                    return getTxInfo(txid);
                }

               
            }

            return false;

        }

        //获取总量
        private static object totalSupply()
        {
            return Storage.Get(Storage.CurrentContext, "totalSupply").AsBigInteger();
        }

        //交易
        private static bool transfer(byte[] from, byte[] to, BigInteger value)
        {
            if (value <= 0)
                return false;
            if (from == to)
                return true;
            if (from.Length > 0)
            {
                var keyFrom = new byte[] {0x11}.Concat(from);
                BigInteger from_value = Storage.Get(Storage.CurrentContext, keyFrom).AsBigInteger();
                if (from_value < value)
                    return false;
                if (from_value == value)
                    Storage.Delete(Storage.CurrentContext, keyFrom);
                else
                {
                    Storage.Put(Storage.CurrentContext, keyFrom, from_value - value);
                }
            }

            if (to.Length > 0)
            {
                var keyTo = new byte[] {0x11}.Concat(to);
                BigInteger to_value = Storage.Get(Storage.CurrentContext, keyTo).AsBigInteger();
                Storage.Put(Storage.CurrentContext, keyTo, to_value + value);
            }

            setTxInfo(from, to, value);
            Transferred(from, to, value);
            return true;
        }

        private static void setTxInfo(byte[] from, byte[] to, BigInteger value)
        {
            TransferInfo info = new TransferInfo();
            info.@from = from;
            info.to = to;
            info.value = value;
            byte[] txInfo = Helper.Serialize(info);
            var txid = (ExecutionEngine.ScriptContainer as Transaction).Hash;
            var keyTxid = new byte[] {0x13}.Concat(txid);
            Storage.Put(Storage.CurrentContext, keyTxid, txInfo);
        }

        private static object balanceOf(byte[] who)
        {
            var keyAddress = new byte[] {0x11}.Concat(who);
            return Storage.Get(Storage.CurrentContext, keyAddress).AsBigInteger();
        }

        private static TransferInfo getTxInfo(byte[] txid)
        {
            byte[] keyTxid=new byte[] {0x13}.Concat(txid);
            byte[] v = Storage.Get(Storage.CurrentContext, keyTxid);
            if (v.Length == 0)
                return null;
            return Helper.Deserialize(v) as TransferInfo;
        }

        public static bool IsPayable(byte[] to)
        {
            var c = Blockchain.GetContract(to);
            if (c.Equals(null))
                return true;
            return c.IsPayable;
        }
    }
}

```


发布合约
