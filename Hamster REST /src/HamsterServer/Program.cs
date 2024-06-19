using CommandLine;
using HSRM.CS.DistributedSystems.Hamster;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

Parser.Default.ParseArguments<ServerOptions>(args)
    .WithParsed(options =>
    {
        builder.WebHost.UseKestrel(kestrel =>
        {
            kestrel.Listen(IPAddress.Parse(options.IPAddress), options.Port);
        });
        HamsterManagement.FeedSuccessProbability = options.FeedSuccessProbability;
    })
    .WithNotParsed(_ => Environment.Exit(2));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IHamsterManagement, HamsterManagement>();

var app = builder.Build();
app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI();
app.Run();