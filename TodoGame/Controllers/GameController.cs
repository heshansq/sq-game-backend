using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using TodoGame.Events;
using TodoGame.Models;
using TodoGame.Services;
using TodoGame.Services.Impl;
using Nethereum.Web3;
using Nethereum.Signer;
using Nethereum.Web3.Accounts;
using Newtonsoft.Json.Linq;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Hex.HexConvertors.Extensions;
using System.Numerics;

namespace TodoGame.Controllers;

[ApiController]
[Route("[controller]")]
//[Authorize]
public class GameController : ControllerBase
{
    private readonly IPokeDexService _pokeDexService;
    private readonly IUserService _userService;
    private readonly IGameService _gameService;

    private readonly ISignalRMessageService _signalRMessageService;

    //private HubConnection hubConnection;

    public GameController(IPokeDexService pokeDexService, IUserService userService, IGameService gameService, ISignalRMessageService signalRMessageService) {
        _pokeDexService = pokeDexService;
        _userService = userService;
        _gameService = gameService;
        _signalRMessageService = signalRMessageService;
    }

    [HttpGet]
    [AllowAnonymous]
    [Route("getpokedexes")]
    public IActionResult getAllPokeDices()
    {
        return Ok(_pokeDexService.GetPokeDices());
    }

    [HttpGet("{userid}")]
    public ActionResult<List<Game>> getUserGameList(string userid)
    {
        return _gameService.listUserGame(userid);
    }

    [HttpGet("getbalance/{publicaddress}")]
    [AllowAnonymous]
    public async Task<IActionResult> getAccPkdtBalance(string publicaddress)
    {
        var url = "https://rpc.ankr.com/eth_goerli";
        var privateKey = "933a0f6368834b5e3a6e065c729fe09f08c31f4366f4f7e04bd59ef54caf89a2";
        //var chainId = 1337;
        var chainId = 5;
        var moneyContractAddress = "0x8a75f9b59dca3cf0f1bedcab737643283806bbfd";

        var account = new Account(privateKey, chainId);
        var web3 = new Web3(account, url);

        var abi = @"[{'inputs':[{'internalType':'address','name':'_owner','type':'address'},{'internalType':'address','name':'_spender','type':'address'},{'internalType':'uint256','name':'_value','type':'uint256'}],'name':'approveSpenderFromOwner','outputs':[{'internalType':'uint256','name':'balance','type':'uint256'}],'stateMutability':'nonpayable','type':'function'},{'inputs':[{'internalType':'address','name':'_spender','type':'address'},{'internalType':'uint256','name':'_value','type':'uint256'}],'name':'approveSpenderSection','outputs':[{'internalType':'uint256','name':'balance','type':'uint256'}],'stateMutability':'nonpayable','type':'function'},{'inputs':[],'name':'balanceOf','outputs':[{'internalType':'uint256','name':'balance','type':'uint256'}],'stateMutability':'nonpayable','type':'function'},{'inputs':[],'name':'buy','outputs':[],'stateMutability':'payable','type':'function'},{'inputs':[{'internalType':'address','name':'_useraddress','type':'address'}],'name':'currentBalanceUser','outputs':[{'internalType':'uint256','name':'balance','type':'uint256'}],'stateMutability':'nonpayable','type':'function'},{'inputs':[{'internalType':'address','name':'_owner','type':'address'},{'internalType':'address','name':'_spender','type':'address'}],'name':'getAllowance','outputs':[{'internalType':'uint256','name':'balance','type':'uint256'}],'stateMutability':'nonpayable','type':'function'},{'inputs':[{'internalType':'uint256','name':'_amount','type':'uint256'}],'name':'sell','outputs':[],'stateMutability':'payable','type':'function'},{'inputs':[],'stateMutability':'nonpayable','type':'constructor'},{'anonymous':false,'inputs':[{'indexed':false,'internalType':'uint256','name':'amount','type':'uint256'}],'name':'Bought','type':'event'},{'inputs':[],'name':'currentBalance','outputs':[{'internalType':'uint256','name':'balance','type':'uint256'}],'stateMutability':'nonpayable','type':'function'},{'inputs':[],'name':'getAmountCheck','outputs':[{'internalType':'uint256','name':'amount','type':'uint256'}],'stateMutability':'payable','type':'function'},{'anonymous':false,'inputs':[{'indexed':false,'internalType':'uint256','name':'amount','type':'uint256'}],'name':'Sold','type':'event'},{'inputs':[{'internalType':'address','name':'_to','type':'address'},{'internalType':'address','name':'_from','type':'address'},{'internalType':'uint256','name':'_amount','type':'uint256'}],'name':'transferSingleTokenToWinner','outputs':[{'internalType':'uint256','name':'balance','type':'uint256'}],'stateMutability':'nonpayable','type':'function'},{'inputs':[{'internalType':'address','name':'_to','type':'address'},{'internalType':'address','name':'_from','type':'address'},{'internalType':'address','name':'_spender','type':'address'},{'internalType':'uint256','name':'_amount','type':'uint256'}],'name':'transferSingleTokenToWinnerWithSpender','outputs':[{'internalType':'uint256','name':'balance','type':'uint256'}],'stateMutability':'nonpayable','type':'function'},{'inputs':[],'name':'decimals','outputs':[{'internalType':'uint8','name':'','type':'uint8'}],'stateMutability':'view','type':'function'},{'inputs':[],'name':'name','outputs':[{'internalType':'string','name':'','type':'string'}],'stateMutability':'view','type':'function'},{'inputs':[],'name':'symbol','outputs':[{'internalType':'string','name':'','type':'string'}],'stateMutability':'view','type':'function'},{'inputs':[],'name':'token','outputs':[{'internalType':'contractToken','name':'','type':'address'}],'stateMutability':'view','type':'function'}]";
        var contract = web3.Eth.GetContract(abi, moneyContractAddress);

        var currentBalanceFunction = contract.GetFunction("currentBalanceUser");
        uint balance = await currentBalanceFunction.CallAsync<uint>(publicaddress);

        return Ok(balance);
    }

    [HttpGet("transfertoken/{fromAddress}/{toAddress}/{spender}")]
    [AllowAnonymous]
    public async Task<IActionResult> transferCoinsToWinner(string fromAddress, string toAddress, string spender)
    {

        var url = "https://rpc.ankr.com/eth_goerli";
        var privateKey = "933a0f6368834b5e3a6e065c729fe09f08c31f4366f4f7e04bd59ef54caf89a2";
        var moneyContractAddress = "0x8a75f9b59dca3cf0f1bedcab737643283806bbfd";
        //var chainId = 1337;
        var chainId = 5;

        var account = new Account(privateKey, chainId);
        var web3 = new Web3(account, url);

        var abi = @"[{'inputs':[{'internalType':'address','name':'_owner','type':'address'},{'internalType':'address','name':'_spender','type':'address'},{'internalType':'uint256','name':'_value','type':'uint256'}],'name':'approveSpenderFromOwner','outputs':[{'internalType':'uint256','name':'balance','type':'uint256'}],'stateMutability':'nonpayable','type':'function'},{'inputs':[{'internalType':'address','name':'_spender','type':'address'},{'internalType':'uint256','name':'_value','type':'uint256'}],'name':'approveSpenderSection','outputs':[{'internalType':'uint256','name':'balance','type':'uint256'}],'stateMutability':'nonpayable','type':'function'},{'inputs':[],'name':'balanceOf','outputs':[{'internalType':'uint256','name':'balance','type':'uint256'}],'stateMutability':'nonpayable','type':'function'},{'inputs':[],'name':'buy','outputs':[],'stateMutability':'payable','type':'function'},{'inputs':[{'internalType':'address','name':'_useraddress','type':'address'}],'name':'currentBalanceUser','outputs':[{'internalType':'uint256','name':'balance','type':'uint256'}],'stateMutability':'nonpayable','type':'function'},{'inputs':[{'internalType':'address','name':'_owner','type':'address'},{'internalType':'address','name':'_spender','type':'address'}],'name':'getAllowance','outputs':[{'internalType':'uint256','name':'balance','type':'uint256'}],'stateMutability':'nonpayable','type':'function'},{'inputs':[{'internalType':'uint256','name':'_amount','type':'uint256'}],'name':'sell','outputs':[],'stateMutability':'payable','type':'function'},{'inputs':[],'stateMutability':'nonpayable','type':'constructor'},{'anonymous':false,'inputs':[{'indexed':false,'internalType':'uint256','name':'amount','type':'uint256'}],'name':'Bought','type':'event'},{'inputs':[],'name':'currentBalance','outputs':[{'internalType':'uint256','name':'balance','type':'uint256'}],'stateMutability':'nonpayable','type':'function'},{'inputs':[],'name':'getAmountCheck','outputs':[{'internalType':'uint256','name':'amount','type':'uint256'}],'stateMutability':'payable','type':'function'},{'anonymous':false,'inputs':[{'indexed':false,'internalType':'uint256','name':'amount','type':'uint256'}],'name':'Sold','type':'event'},{'inputs':[{'internalType':'address','name':'_to','type':'address'},{'internalType':'address','name':'_from','type':'address'},{'internalType':'uint256','name':'_amount','type':'uint256'}],'name':'transferSingleTokenToWinner','outputs':[{'internalType':'uint256','name':'balance','type':'uint256'}],'stateMutability':'nonpayable','type':'function'},{'inputs':[{'internalType':'address','name':'_to','type':'address'},{'internalType':'address','name':'_from','type':'address'},{'internalType':'address','name':'_spender','type':'address'},{'internalType':'uint256','name':'_amount','type':'uint256'}],'name':'transferSingleTokenToWinnerWithSpender','outputs':[{'internalType':'uint256','name':'balance','type':'uint256'}],'stateMutability':'nonpayable','type':'function'},{'inputs':[],'name':'decimals','outputs':[{'internalType':'uint8','name':'','type':'uint8'}],'stateMutability':'view','type':'function'},{'inputs':[],'name':'name','outputs':[{'internalType':'string','name':'','type':'string'}],'stateMutability':'view','type':'function'},{'inputs':[],'name':'symbol','outputs':[{'internalType':'string','name':'','type':'string'}],'stateMutability':'view','type':'function'},{'inputs':[],'name':'token','outputs':[{'internalType':'contractToken','name':'','type':'address'}],'stateMutability':'view','type':'function'}]";
        var contract = web3.Eth.GetContract(abi, moneyContractAddress);

        //var currentBalanceFunction = contract.GetFunction("transferSingleTokenToWinner");
        //uint balance = await currentBalanceFunction.CallAsync<uint>(toAddress, fromAddress, 1);

        var transferFundFunction = contract.GetFunction("transferSingleTokenToWinnerWithSpender");

        var gas = new HexBigInteger(100000);
        web3.TransactionManager.UseLegacyAsDefault = true;

        var txnReceipt = await transferFundFunction.SendTransactionAsync(spender, gas, null, toAddress, fromAddress, spender, 1);
        return Ok(txnReceipt);

    }

    [HttpPost]
    [AllowAnonymous]
    public IActionResult createNewGame(GameDto gameData)
    {
        Game gameSt = new Game();
        gameSt.type = gameData.type;
        gameSt.status = gameData.status;
        gameSt.gamename = gameData.gamename;

        User gameStartUser = _userService.GetUser(gameData.gamestartuser);
        gameSt.gamestartuser = gameStartUser;

        if (gameStartUser.tickets == null || gameStartUser.tickets == 0)
        {
            return new ObjectResult("User dont have tickets to start a game!") { StatusCode = 403 };
        }

        if (gameData.gameopponent != null)
        {
            User gameOpUser = _userService.GetUser(gameData.gameopponent);
            gameSt.gameopponent = gameOpUser;

            var opuserid = "";

            if (gameOpUser.connectionid != null)
            {
                opuserid = gameOpUser.connectionid;
            }

            SignalRMessage message = new SignalRMessage();
            message.messageType = "gameStart";
            message.startuserid = gameStartUser.connectionid.ToString();
            message.opuserid = opuserid.ToString();
            message.message = "startgame";
            _signalRMessageService.sendGameStatusNotificationAsync(message);
            
        }

        var ticketAmt = gameStartUser.tickets != 0 ? gameStartUser.tickets - 1 : 0;
        UpdateResult userUpdate = _gameService.changeTickets(gameData.gamestartuser, ticketAmt);

        Game saveGame = _gameService.createGame(gameSt);

        return Ok(saveGame);
    }
}

