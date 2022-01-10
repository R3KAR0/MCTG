using MCTG_AutomatedTests;
using MonsterTradingCardsGame.DataLayer.DTO;
using Serilog;
using Serilog.Events;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.File("..\\Test-Log.txt")
    .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Debug)
    .CreateLogger();

Console.WriteLine("AUTOMATED TESTS FOR MCTG");
Log.Information("Starting tests of MCTG!");

HttpClient client = new HttpClient();


Log.Information("*************************************** USER TESTS START ****************************************************");
Log.Information("TEST 1 Register User, EXPECTED: 200 OK!");


var content = new StringContent("{\"username\":\"christian\",\"password\":\"fhtechnikum\"}", Encoding.UTF8, "application/json");
string url = "http://127.0.0.1:8000/users";
var response = await client.PostAsync(url, content);

if(response.StatusCode == HttpStatusCode.Created)
{
    Log.Information($"Received: {response.StatusCode} -> Test Passed!");
    Console.WriteLine();
}
else
{
    Log.Fatal($"Received: {response.StatusCode} -> Test Failed!");
    Console.WriteLine();
}


Log.Information("TEST 2 Register User Again, EXPECTED: 409 Conflict!");
content = new StringContent("{\"username\":\"christian\",\"password\":\"fhtechnikum\"}", Encoding.UTF8, "application/json");
url = "http://127.0.0.1:8000/users";
response = await client.PostAsync(url, content);

if (response.StatusCode == HttpStatusCode.Conflict)
{
    Log.Information($"Received: {response.StatusCode} -> Test Passed!");
    Console.WriteLine();
}
else
{
    Log.Fatal($"Received: {response.StatusCode} -> Test Failed!");
    Console.WriteLine();
}


Log.Information("TEST 3 ValidLogin, EXPECTED: 200 OK and token!");
content = new StringContent("{\"username\":\"christian\",\"password\":\"fhtechnikum\"}", Encoding.UTF8, "application/json");
url = "http://127.0.0.1:8000/users/login";
response = await client.PostAsync(url, content);

var resContent =  response.Content.ReadAsStringAsync().Result;

if (response.StatusCode == HttpStatusCode.OK && resContent.Length > 0)
{
    Log.Information($"Received: {response.StatusCode} Content: {resContent} -> Test Passed!");
    Console.WriteLine();
}
else
{
    Log.Fatal($"Received: {response.StatusCode} -> Test Failed!");
    Console.WriteLine();
}

var tokenDTO = JsonSerializer.Deserialize<LoginResponseDTO>(resContent);
var token = tokenDTO?.Token;




Log.Information("TEST 4 InvalidLogin, EXPECTED: 403 Forbidden");
content = new StringContent("{\"username\":\"christian\",\"password\":\"test\"}", Encoding.UTF8, "application/json");
url = "http://127.0.0.1:8000/users/login";
response = await client.PostAsync(url, content);

if (response.StatusCode == HttpStatusCode.Forbidden)
{
    Log.Information($"Received: {response.StatusCode} -> Test Passed!");
    Console.WriteLine();
}
else
{
    Log.Fatal($"Received: {response.StatusCode} -> Test Failed!");
    Console.WriteLine();
}


Log.Information("TEST 5 GetUserPage, EXPECTED: 200 OK");
url = "http://127.0.0.1:8000/users";
var request = new HttpRequestMessage()
{
    RequestUri = new Uri(url),
    Method = HttpMethod.Get,
};
request.Headers.TryAddWithoutValidation("Authorization", token.ToString());

response = await client.SendAsync(request);

resContent = await response.Content.ReadAsStringAsync();

if (response.StatusCode == HttpStatusCode.OK && resContent.Length > 0)
{
    Log.Information($"Received: {response.StatusCode} Content: {resContent} -> Test Passed!");
    Console.WriteLine();
}
else
{
    Log.Fatal($"Received: {response.StatusCode} -> Test Failed!");
    Console.WriteLine();
}


Log.Information("TEST 6 UpdateUserProfile, EXPECTED: 200 OK");
url = "http://127.0.0.1:8000/users";

content = new StringContent("{\"newpassword\": null,\"newdescription\": \"Updated\",\"newpicture\": null}", Encoding.UTF8, "application/json");

request = new HttpRequestMessage()
{
    RequestUri = new Uri(url),
    Method = HttpMethod.Put,
};
request.Headers.TryAddWithoutValidation("Authorization", token);
request.Content = content;
response = await client.SendAsync(request);

resContent = await response.Content.ReadAsStringAsync();

if (response.StatusCode == HttpStatusCode.OK && resContent.Length > 0)
{
    Log.Information($"Received: {response.StatusCode} Content: {resContent} -> Test Passed!");
    Console.WriteLine();
}
else
{
    Log.Fatal($"Received: {response.StatusCode} -> Test Failed!");
    Console.WriteLine();
}

Log.Information("******************************** USER TESTS END ***********************************************");

Log.Information("************************************ CARDS/PACKAGES/DECKS TESTS START **********************************************");
Log.Information("TEST 7 BuyPackages, EXPECTED: 200 OK");
url = "http://127.0.0.1:8000/packages/buy";
request = new HttpRequestMessage()
{
    RequestUri = new Uri(url),
    Method = HttpMethod.Post,
};
request.Headers.TryAddWithoutValidation("Authorization", token);

bool success = true;
response = client.Send(request);
if (response.StatusCode != HttpStatusCode.OK) success = false;

request = new HttpRequestMessage()
{
    RequestUri = new Uri(url),
    Method = HttpMethod.Post,
};
request.Headers.TryAddWithoutValidation("Authorization", token);

response = client.Send(request);
if (response.StatusCode != HttpStatusCode.OK) success = false;

request = new HttpRequestMessage()
{
    RequestUri = new Uri(url),
    Method = HttpMethod.Post,
};
request.Headers.TryAddWithoutValidation("Authorization", token);

response = client.Send(request);
if (response.StatusCode != HttpStatusCode.OK) success = false;


request = new HttpRequestMessage()
{
    RequestUri = new Uri(url),
    Method = HttpMethod.Post,
};
request.Headers.TryAddWithoutValidation("Authorization", token);
response = client.Send(request);

if (success)
{
    Log.Information($"Received: {response.StatusCode} -> Test Passed!");
    Console.WriteLine();
}
else
{
    Log.Fatal($"Received: {response.StatusCode} -> Test Failed!");
    Console.WriteLine();
}

Log.Information("TEST 8 BuyPackagesWithNoCoins, EXPECTED: 200 OK");
url = "http://127.0.0.1:8000/packages/buy";
request = new HttpRequestMessage()
{
    RequestUri = new Uri(url),
    Method = HttpMethod.Post,
};
request.Headers.TryAddWithoutValidation("Authorization", token);

response = client.Send(request);

if (response.StatusCode == HttpStatusCode.PaymentRequired)
{
    Log.Information($"Received: {response.StatusCode} -> Test Passed!");
    Console.WriteLine();
}
else
{
    Log.Fatal($"Received: {response.StatusCode} -> Test Failed!");
    Console.WriteLine();
}




Log.Information("TEST 9 GetAllCardsOfUser, EXPECTED: 200 OK");
url = "http://127.0.0.1:8000/cards";
request = new HttpRequestMessage()
{
    RequestUri = new Uri(url),
    Method = HttpMethod.Get,
};
request.Headers.TryAddWithoutValidation("Authorization", token);

response = client.Send(request);
resContent = await response.Content.ReadAsStringAsync();

if (response.StatusCode == HttpStatusCode.OK && resContent.Length > 0)
{
    Log.Information($"Received: {response.StatusCode} Content: {resContent} -> Test Passed!");
    Console.WriteLine();
}
else
{
    Log.Fatal($"Received: {response.StatusCode} -> Test Failed!");
    Console.WriteLine();
}

var cards = JsonSerializer.Deserialize<UserCardsDTO>(resContent);


Log.Information("TEST 10 GetDecksOfUser, EXPECTED: 200 OK");
url = "http://127.0.0.1:8000/decks";
request = new HttpRequestMessage()
{
    RequestUri = new Uri(url),
    Method = HttpMethod.Get,
};
request.Headers.TryAddWithoutValidation("Authorization", token);

response = client.Send(request);
resContent = response.Content.ReadAsStringAsync().Result;

if (response.StatusCode == HttpStatusCode.OK && resContent.Length > 0)
{
    Log.Information($"Received: {response.StatusCode} -> Test Passed!");
    Console.WriteLine();
}
else
{
    Log.Fatal($"Received: {response.StatusCode} -> Test Failed!");
    Console.WriteLine();
}


Log.Information("TEST 11 CreateDeck, EXPECTED: 200 OK");
url = "http://127.0.0.1:8000/decks";
content = new StringContent("{\"description\": \"StarterDeck\"}", Encoding.UTF8, "application/json");
request = new HttpRequestMessage()
{
    RequestUri = new Uri(url),
    Method = HttpMethod.Post,
};
request.Content = content;
request.Headers.TryAddWithoutValidation("Authorization", token);

response = client.Send(request);

if (response.StatusCode == HttpStatusCode.Created)
{
    Log.Information($"Received: {response.StatusCode} -> Test Passed!");
    Console.WriteLine();
}
else
{
    Log.Fatal($"Received: {response.StatusCode} -> Test Failed!");
    Console.WriteLine();
}


Log.Information("TEST 12 GetNewDeck, EXPECTED: 200 OK");
url = "http://127.0.0.1:8000/decks";
request = new HttpRequestMessage()
{
    RequestUri = new Uri(url),
    Method = HttpMethod.Get,
};
request.Headers.TryAddWithoutValidation("Authorization", token);

response = client.Send(request);
resContent = response.Content.ReadAsStringAsync().Result;

if (response.StatusCode == HttpStatusCode.OK && resContent.Length > 0)
{
    Log.Information($"Received: {response.StatusCode} Content: {resContent} -> Test Passed!");
    Console.WriteLine();
}
else
{
    Log.Fatal($"Received: {response.StatusCode} -> Test Failed!");
    Console.WriteLine();
}

var decks = JsonSerializer.Deserialize<DecksDTO>(resContent);




Log.Information("TEST 13 AddCardsToDeck, EXPECTED: 200 Accepted");

if (cards == null || decks == null) throw new Exception();

List<Guid> ids = new List<Guid>();
for (int i = 0; i < 4; i++)
{
    ids.Add(cards.Cards[i].Id);
}
var cardIdListDeckDTO = new CardIdListDeckDTO(ids, decks.Decks[0].Id);
url = "http://127.0.0.1:8000/decks";
content = new StringContent(JsonSerializer.Serialize(cardIdListDeckDTO), Encoding.UTF8, "application/json");
request = new HttpRequestMessage()
{
    RequestUri = new Uri(url),
    Method = HttpMethod.Put,
};
request.Content = content;
request.Headers.TryAddWithoutValidation("Authorization", token);

response = client.Send(request);

if (response.StatusCode == HttpStatusCode.Accepted)
{
    Log.Information($"Received: {response.StatusCode} -> Test Passed!");
    Console.WriteLine();
}
else
{
    Log.Fatal($"Received: {response.StatusCode} -> Test Failed!");
    Console.WriteLine();
}

Log.Information("TEST 14 Select Deck, EXPECTED: 200 Accecpted");

url = "http://127.0.0.1:8000/decks/select";

DeckSelectionDTO selection = new DeckSelectionDTO(cardIdListDeckDTO.DeckId);

content = new StringContent(JsonSerializer.Serialize(selection), Encoding.UTF8, "application/json");
request = new HttpRequestMessage()
{
    RequestUri = new Uri(url),
    Method = HttpMethod.Post,
};
request.Content = content;
request.Headers.TryAddWithoutValidation("Authorization", token);

response = client.Send(request);

if (response.StatusCode == HttpStatusCode.Accepted)
{
    Log.Information($"Received: {response.StatusCode} -> Test Passed!");
    Console.WriteLine();
}
else
{
    Log.Fatal($"Received: {response.StatusCode} -> Test Failed!"); ;
    Console.WriteLine();
}


Log.Information("TEST 14 Get Selected Deck, EXPECTED: 200 OK");

url = "http://127.0.0.1:8000/decks/select";
request = new HttpRequestMessage()
{
    RequestUri = new Uri(url),
    Method = HttpMethod.Get,
};
request.Headers.TryAddWithoutValidation("Authorization", token);

response = client.Send(request);
resContent = response.Content.ReadAsStringAsync().Result;
if (response.StatusCode == HttpStatusCode.OK && resContent.Length > 0)
{
    Log.Information($"Received: {response.StatusCode} Content: {resContent} -> Test Passed!");
    Console.WriteLine();
}
else
{
    Log.Fatal($"Received: {response.StatusCode} -> Test Failed!");
    Console.WriteLine();
}
Log.Information("************************************ CARDS/PACKAGES/DECKS TESTS END **********************************************");

Log.Information("************************************* CONFIGURE OPPONENT START *********************************************");

Log.Information("Create Opponent User");

var oppnent = new StringContent("{\"username\":\"test\",\"password\":\"test\"}", Encoding.UTF8, "application/json");
string oppnenturl = "http://127.0.0.1:8000/users";
var oppnentresponse =  await client.PostAsync(oppnenturl, oppnent);

Log.Information("Get Opponent Token, EXPECTED: 200 OK and token!");
url = "http://127.0.0.1:8000/users/login";
response = client.PostAsync(url, oppnent).Result;

var resContentoppnent =  response.Content.ReadAsStringAsync().Result;
var oppnenttokenDTO = JsonSerializer.Deserialize<LoginResponseDTO>(resContentoppnent);
var opponenttoken = oppnenttokenDTO.Token;

Log.Information("Opponent BuyPackages, EXPECTED: 200 OK");
url = "http://127.0.0.1:8000/packages/buy";
request = new HttpRequestMessage()
{
    RequestUri = new Uri(url),
    Method = HttpMethod.Post,
};
request.Headers.TryAddWithoutValidation("Authorization", opponenttoken);

response = client.Send(request);

request = new HttpRequestMessage()
{
    RequestUri = new Uri(url),
    Method = HttpMethod.Post,
};
request.Headers.TryAddWithoutValidation("Authorization", opponenttoken);

response = client.Send(request);

request = new HttpRequestMessage()
{
    RequestUri = new Uri(url),
    Method = HttpMethod.Post,
};
request.Headers.TryAddWithoutValidation("Authorization", opponenttoken);

response = client.Send(request);

request = new HttpRequestMessage()
{
    RequestUri = new Uri(url),
    Method = HttpMethod.Post,
};
request.Headers.TryAddWithoutValidation("Authorization", opponenttoken);
response = client.Send(request);


Log.Information("Get Opponent Cards, EXPECTED: 200 OK");
url = "http://127.0.0.1:8000/cards";
request = new HttpRequestMessage()
{
    RequestUri = new Uri(url),
    Method = HttpMethod.Get,
};
request.Headers.TryAddWithoutValidation("Authorization", opponenttoken);

response = client.Send(request);
resContent =  response.Content.ReadAsStringAsync().Result;

var opponentcards = JsonSerializer.Deserialize<UserCardsDTO>(resContent);


Log.Information("Create OpponentDeck, EXPECTED: 200 OK");
url = "http://127.0.0.1:8000/decks";
content = new StringContent("{\"description\": \"StarterDeck\"}", Encoding.UTF8, "application/json");
request = new HttpRequestMessage()
{
    RequestUri = new Uri(url),
    Method = HttpMethod.Post,
};
request.Content = content;
request.Headers.TryAddWithoutValidation("Authorization", opponenttoken);

response = client.Send(request);

Log.Information("Get OpponentDeck, EXPECTED: 200 OK");
url = "http://127.0.0.1:8000/decks";
request = new HttpRequestMessage()
{
    RequestUri = new Uri(url),
    Method = HttpMethod.Get,
};
request.Headers.TryAddWithoutValidation("Authorization", opponenttoken);

response = client.Send(request);
resContent = response.Content.ReadAsStringAsync().Result;

var opponentdecks = JsonSerializer.Deserialize<DecksDTO>(resContent);




Log.Information("AddCardsToOpponentDeck, EXPECTED: 200 Accepted");

if (opponentcards == null || decks == null) throw new Exception();

List<Guid> oids = new List<Guid>();
for (int i = 0; i < 4; i++)
{
    oids.Add(opponentcards.Cards[i].Id);
}
var opponentcardIdListDeckDTO = new CardIdListDeckDTO(oids, opponentdecks.Decks[0].Id);
url = "http://127.0.0.1:8000/decks";
content = new StringContent(JsonSerializer.Serialize(opponentcardIdListDeckDTO), Encoding.UTF8, "application/json");
request = new HttpRequestMessage()
{
    RequestUri = new Uri(url),
    Method = HttpMethod.Put,
};
request.Content = content;
request.Headers.TryAddWithoutValidation("Authorization", opponenttoken);

response = client.Send(request);

Log.Information("Select Opponent Deck, EXPECTED: 200 Accecpted");

url = "http://127.0.0.1:8000/decks/select";

DeckSelectionDTO oponnentselection = new DeckSelectionDTO(opponentcardIdListDeckDTO.DeckId);

content = new StringContent(JsonSerializer.Serialize(oponnentselection), Encoding.UTF8, "application/json");
request = new HttpRequestMessage()
{
    RequestUri = new Uri(url),
    Method = HttpMethod.Post,
};
request.Content = content;
request.Headers.TryAddWithoutValidation("Authorization", opponenttoken);

response = client.Send(request);

Log.Information("************************************* CONFIGURE OPPONENT END *********************************************");

Log.Information("************************************* STATS/BATTLE TESTS START *********************************************");

Log.Information("TEST 15 GET Stats, EXPECTED: 200 OK");
url = "http://127.0.0.1:8000/stats";

request = new HttpRequestMessage()
{
    RequestUri = new Uri(url),
    Method = HttpMethod.Get,
};
request.Headers.TryAddWithoutValidation("Authorization", token);
response =  client.Send(request);

resContent = response.Content.ReadAsStringAsync().Result;

if (response.StatusCode == HttpStatusCode.OK && resContent.Length > 0)
{
    Log.Information($"Received: {response.StatusCode} Content: {resContent} -> Test Passed!");
    Console.WriteLine();
}
else
{
    Log.Fatal($"Received: {response.StatusCode} -> Test Failed!");
    Console.WriteLine();
}


Log.Information("TEST 16 GET Scoreboard, EXPECTED: 200 OK");
url = "http://127.0.0.1:8000/stats/scoreboard";

request = new HttpRequestMessage()
{
    RequestUri = new Uri(url),
    Method = HttpMethod.Get,
};
request.Headers.TryAddWithoutValidation("Authorization", token);
response = client.Send(request);

resContent = response.Content.ReadAsStringAsync().Result;

if (response.StatusCode == HttpStatusCode.OK && resContent.Length > 0)
{
    Log.Information($"Received: {response.StatusCode} Content: {resContent} -> Test Passed!");
    Console.WriteLine();
}
else
{
    Log.Fatal($"Received: {response.StatusCode} -> Test Failed!");
    Console.WriteLine();
}



Log.Information("TEST 17 BATTLE, EXPECTED: 200 OK");
var url2 = "http://127.0.0.1:8000/battle";


var request1 = new HttpRequestMessage()
{
    RequestUri = new Uri(url2),
    Method = HttpMethod.Post,
};
request1.Headers.TryAddWithoutValidation("Authorization", token);


Thread T = new Thread(new ParameterizedThreadStart(BattleHandler.Battle));
T.Start(request1);

Thread.Sleep(2000);
var request2 = new HttpRequestMessage()
{
    RequestUri = new Uri(url2),
    Method = HttpMethod.Post,
};
request2.Headers.TryAddWithoutValidation("Authorization", opponenttoken);
var r = client.Send(request2);

resContent = r.Content.ReadAsStringAsync().Result;

if (r.StatusCode == HttpStatusCode.OK && resContent.Length > 0)
{
    Log.Information($"Received: {r.StatusCode} Content: {resContent} -> Test Passed!");
    Console.WriteLine();
}
else
{
    Log.Fatal($"Received: {r.StatusCode} -> Test Failed!");
    Console.WriteLine();
}

Log.Information("TEST 18 GET Stats, EXPECTED: 200 OK");
url = "http://127.0.0.1:8000/stats";

request = new HttpRequestMessage()
{
    RequestUri = new Uri(url),
    Method = HttpMethod.Get,
};
request.Headers.TryAddWithoutValidation("Authorization", token);
response = client.Send(request);

resContent = response.Content.ReadAsStringAsync().Result;

if (response.StatusCode == HttpStatusCode.OK && resContent.Length > 0)
{
    Log.Information($"Received: {response.StatusCode} Content: {resContent} -> Test Passed!");
    Console.WriteLine();
}
else
{
    Log.Fatal($"Received: {response.StatusCode} -> Test Failed!");
    Console.WriteLine();
}


Log.Information("TEST 19 GET Stats Opponent, EXPECTED: 200 OK");
url = "http://127.0.0.1:8000/stats";

request = new HttpRequestMessage()
{
    RequestUri = new Uri(url),
    Method = HttpMethod.Get,
};
request.Headers.TryAddWithoutValidation("Authorization", opponenttoken);
response = client.Send(request);

resContent = response.Content.ReadAsStringAsync().Result;

if (response.StatusCode == HttpStatusCode.OK && resContent.Length > 0)
{
    Log.Information($"Received: {response.StatusCode} Content: {resContent} -> Test Passed!");
    Console.WriteLine();
}
else
{
    Log.Fatal($"Received: {response.StatusCode} -> Test Failed!");
    Console.WriteLine();
}



Log.Information("************************************* STATS/BATTLE TESTS END *********************************************");

Log.Information("************************************* TRADE TESTS START *********************************************");



Log.Information("TEST 20 GET TradeOffers, EXPECTED: 200 OK");
url = "http://127.0.0.1:8000/tradingoffers";

request = new HttpRequestMessage()
{
RequestUri = new Uri(url),
Method = HttpMethod.Get,
};
request.Headers.TryAddWithoutValidation("Authorization", token);
response = client.Send(request);

resContent = response.Content.ReadAsStringAsync().Result;

if (response.StatusCode == HttpStatusCode.OK && resContent.Length > 0)
{
Log.Information($"Received: {response.StatusCode} Content: {resContent} -> Test Passed!");
Console.WriteLine();
}
else
{
Log.Fatal($"Received: {response.StatusCode} -> Test Failed!");
Console.WriteLine();
}


Log.Information("TEST 21 Create TradeOffers, EXPECTED: 200 OK");
url = "http://127.0.0.1:8000/tradingoffers";

request = new HttpRequestMessage()
{
    RequestUri = new Uri(url),
    Method = HttpMethod.Post,
};
var createTrade = new StringContent($"{{\"cardid\":\"{cards.Cards[0].Id}\",\"desiredType\": \"MONSTER\",\"minDamage\" : 0}}", Encoding.UTF8, "application/json");
request.Content = createTrade;
request.Headers.TryAddWithoutValidation("Authorization", token);
response = client.Send(request);

resContent = response.Content.ReadAsStringAsync().Result;

if (response.StatusCode == HttpStatusCode.Accepted)
{
    Log.Information($"Received: {response.StatusCode} Content: {resContent} -> Test Passed!");
    Console.WriteLine();
}
else
{
    Log.Fatal($"Received: {response.StatusCode} -> Test Failed!");
    Console.WriteLine();
}

request = new HttpRequestMessage()
{
    RequestUri = new Uri(url),
    Method = HttpMethod.Post,
};
createTrade = new StringContent($"{{\"cardid\":\"{cards.Cards[1].Id}\",\"desiredType\": \"MONSTER\",\"minDamage\" : 0}}", Encoding.UTF8, "application/json");
request.Content = createTrade;
request.Headers.TryAddWithoutValidation("Authorization", token);
response = client.Send(request);

resContent = response.Content.ReadAsStringAsync().Result;



Log.Information("TEST 22 GET TradeOffers, EXPECTED: 200 OK");
url = "http://127.0.0.1:8000/tradingoffers";

request = new HttpRequestMessage()
{
    RequestUri = new Uri(url),
    Method = HttpMethod.Get,
};
request.Headers.TryAddWithoutValidation("Authorization", token);
response = client.Send(request);

resContent = response.Content.ReadAsStringAsync().Result;

if (response.StatusCode == HttpStatusCode.OK && resContent.Length > 0)
{
    Log.Information($"Received: {response.StatusCode} Content: {resContent} -> Test Passed!");
    Console.WriteLine();
}
else
{
    Log.Fatal($"Received: {response.StatusCode} -> Test Failed!");
    Console.WriteLine();
}




Log.Information("TEST 23 Try Trade, EXPECTED: Should fail");

url = "http://127.0.0.1:8000/tradingoffers/trade";

request = new HttpRequestMessage()
{
    RequestUri = new Uri(url),
    Method = HttpMethod.Post,
};

var buyercardFAIL = cards.Cards.Where(card => card.Type == MonsterTradingCardsGame.Models.EType.MONSTER).First();
var trade = new StringContent($"{{\"tradeid\":\"{cards.Cards[0].Id}\",\"buyerid\": \"{buyercardFAIL.Id}\"}}", Encoding.UTF8, "application/json");
request.Content = trade;
request.Headers.TryAddWithoutValidation("Authorization", token);
response = client.Send(request);

resContent = response.Content.ReadAsStringAsync().Result;

if (response.StatusCode != HttpStatusCode.OK)
{
    Log.Information($"Received: {response.StatusCode} Content: {resContent} -> Test Passed!");
    Console.WriteLine();
}
else
{
    Log.Fatal($"Received: {response.StatusCode} -> Test Failed!");
    Console.WriteLine();
}



Log.Information("TEST 24 Trade, EXPECTED: ACCEPTED");

url = "http://127.0.0.1:8000/tradingoffers/trade";

request = new HttpRequestMessage()
{
    RequestUri = new Uri(url),
    Method = HttpMethod.Post,
};

var buyercard = opponentcards.Cards.Where(card => card.Type == MonsterTradingCardsGame.Models.EType.MONSTER).First();
var tradeOpponent = new StringContent($"{{\"tradeid\":\"{cards.Cards[0].Id}\",\"buyerid\": \"{buyercard.Id}\"}}", Encoding.UTF8, "application/json");
request.Content = tradeOpponent;
request.Headers.TryAddWithoutValidation("Authorization", opponenttoken);
response = client.Send(request);

resContent = response.Content.ReadAsStringAsync().Result;

if (response.StatusCode == HttpStatusCode.Accepted)
{
    Log.Information($"Received: {response.StatusCode} -> Test Passed!");
    Console.WriteLine();
}
else
{
    Log.Fatal($"Received: {response.StatusCode} -> Test Failed!");
    Console.WriteLine();
}


Log.Information("TEST 25 DELETE Trade, EXPECTED: NoContent");

url = "http://127.0.0.1:8000/tradingoffers";

request = new HttpRequestMessage()
{
    RequestUri = new Uri(url),
    Method = HttpMethod.Delete,
};

var delete = new StringContent($"{{\"cardid\":\"{cards.Cards[1].Id}\"}}", Encoding.UTF8, "application/json");
request.Content = delete;
request.Headers.TryAddWithoutValidation("Authorization", token);
response = client.Send(request);

resContent = response.Content.ReadAsStringAsync().Result;

if (response.StatusCode == HttpStatusCode.NoContent)
{
    Log.Information($"Received: {response.StatusCode} -> Test Passed!");
    Console.WriteLine();
}
else
{
    Log.Fatal($"Received: {response.StatusCode} -> Test Failed!");
    Console.WriteLine();
}



Log.Information("TEST 26 INJECTION TEST, EXPECTED: MUST FAIL!!");
content = new StringContent("{\"username\":\"christian\",\"password\":\"'' OR 1=1\"}", Encoding.UTF8, "application/json");
url = "http://127.0.0.1:8000/users/login";
response = await client.PostAsync(url, content);

var injection = response.Content.ReadAsStringAsync().Result;

if (response.StatusCode != HttpStatusCode.OK && injection.Length == 0)
{
    Log.Information($"Received: {response.StatusCode} -> Test Passed!");
    Console.WriteLine();
}
else
{
    Log.Fatal($"Received: {response.StatusCode} -> Test Failed!");
    Console.WriteLine();
}



Log.Information("TEST 27 Add Coins, EXPECTED: Accepted");
url = "http://127.0.0.1:8000/users/coins";

content = new StringContent("{\"amount\":100}", Encoding.UTF8, "application/json");
request = new HttpRequestMessage()
{
    RequestUri = new Uri(url),
    Method = HttpMethod.Post,
};
request.Headers.TryAddWithoutValidation("Authorization", opponenttoken);
request.Content = content;
response = client.Send(request);

var coins = response.Content.ReadAsStringAsync().Result;

if (response.StatusCode == HttpStatusCode.Accepted)
{
    Log.Information($"Received: {response.StatusCode} -> Test Passed!");
    Console.WriteLine();
}
else
{
    Log.Fatal($"Received: {response.StatusCode} -> Test Failed!");
    Console.WriteLine();
}


Log.Information("TEST 28 Create SellOffer, EXPECTED: Accepted");
url = "http://127.0.0.1:8000/sellingoffers";

request = new HttpRequestMessage()
{
    RequestUri = new Uri(url),
    Method = HttpMethod.Post,
};
var sellID = cards.Cards[4].Id;
var createSell = new StringContent($"{{\"cardid\":\"{sellID}\",\"price\": 5}}", Encoding.UTF8, "application/json");
request.Content = createSell;
request.Headers.TryAddWithoutValidation("Authorization", token);
response = client.Send(request);

resContent = response.Content.ReadAsStringAsync().Result;

if (response.StatusCode == HttpStatusCode.Accepted)
{
    Log.Information($"Received: {response.StatusCode} Content: {resContent} -> Test Passed!");
    Console.WriteLine();
}
else
{
    Log.Fatal($"Received: {response.StatusCode} -> Test Failed!");
    Console.WriteLine();
}


Log.Information("TEST 29 Get SellOffer, EXPECTED: 200 OK");
url = "http://127.0.0.1:8000/sellingoffers";

request = new HttpRequestMessage()
{
    RequestUri = new Uri(url),
    Method = HttpMethod.Get,
};
request.Headers.TryAddWithoutValidation("Authorization", token);
response = client.Send(request);

resContent = response.Content.ReadAsStringAsync().Result;

if (response.StatusCode == HttpStatusCode.OK && resContent.Length > 0)
{
    Log.Information($"Received: {response.StatusCode} Content: {resContent} -> Test Passed!");
    Console.WriteLine();
}
else
{
    Log.Fatal($"Received: {response.StatusCode} -> Test Failed!");
    Console.WriteLine();
}


Log.Information("TEST 30 Buy SellOffer Same Account, EXPECTED: Forbidden");
url = "http://127.0.0.1:8000/sellingoffers/buy";

var buy = new StringContent($"{{\"cardid\":\"{sellID}\"}}", Encoding.UTF8, "application/json");
request = new HttpRequestMessage()
{
    RequestUri = new Uri(url),
    Method = HttpMethod.Post,
};
request.Headers.TryAddWithoutValidation("Authorization", token);
request.Content = buy;
response = client.Send(request);

resContent = response.Content.ReadAsStringAsync().Result;

if (response.StatusCode != HttpStatusCode.Accepted)
{
    Log.Information($"Received: {response.StatusCode} Content: {resContent} -> Test Passed!");
    Console.WriteLine();
}
else
{
    Log.Fatal($"Received: {response.StatusCode} -> Test Failed!");
    Console.WriteLine();
}



Log.Information("TEST 31 Buy SellOffer, EXPECTED: ACCEPTED");
url = "http://127.0.0.1:8000/sellingoffers/buy";

var buyOpponent = new StringContent($"{{\"cardid\":\"{sellID}\"}}", Encoding.UTF8, "application/json");
request = new HttpRequestMessage()
{
    RequestUri = new Uri(url),
    Method = HttpMethod.Post,
};
request.Headers.TryAddWithoutValidation("Authorization", opponenttoken);
request.Content = buyOpponent;
response = client.Send(request);

resContent = response.Content.ReadAsStringAsync().Result;

if (response.StatusCode == HttpStatusCode.Accepted)
{
    Log.Information($"Received: {response.StatusCode} Content: {resContent} -> Test Passed!");
    Console.WriteLine();
}
else
{
    Log.Fatal($"Received: {response.StatusCode} -> Test Failed!");
    Console.WriteLine();
}



Log.Information("TEST 32 RoutingErrors (WrongMethod), EXPECTED: BadRequest");
url = "http://127.0.0.1:8000/sellingoffers/buy";

buyOpponent = new StringContent($"{{\"cardid\":\"{sellID}\"}}", Encoding.UTF8, "application/json");
request = new HttpRequestMessage()
{
    RequestUri = new Uri(url),
    Method = HttpMethod.Get,
};
request.Headers.TryAddWithoutValidation("Authorization", opponenttoken);
request.Content = buyOpponent;
response = client.Send(request);

resContent = response.Content.ReadAsStringAsync().Result;

if (response.StatusCode == HttpStatusCode.BadRequest)
{
    Log.Information($"Received: {response.StatusCode} Content: {resContent} -> Test Passed!");
    Console.WriteLine();
}
else
{
    Log.Fatal($"Received: {response.StatusCode} -> Test Failed!");
    Console.WriteLine();
}
