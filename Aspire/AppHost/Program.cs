var builder = DistributedApplication.CreateBuilder(args);

var redis = builder.AddRedis("redis", 6379)
        .WithRedisInsight(config => { config.WithHostPort(6378); })
    ;

var orleans = builder.AddOrleans("orleans")
        .WithClustering(redis)
        .WithGrainStorage("stocks", redis)
        .WithGrainStorage("orders", redis)
    ;

var silo = builder.AddProject<Projects.Silo>("silos")
        .WaitFor(redis)
        .WithReference(orleans)
    ;

var api = builder.AddProject<Projects.API>("api")
        .WaitFor(silo)
        .WaitFor(redis)
        .WithReference(orleans.AsClient())
        .WithExternalHttpEndpoints()
    ;

_ = builder.AddProject<Projects.Nelogica>("nelogica")
        .WaitFor(silo)
        .WaitFor(api)
        .WithReference(orleans.AsClient())
    ;

builder.Build().Run();