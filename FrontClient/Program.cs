using ServicesManipulation.Messaging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddHostedService<ClientOrderUpdaterConsumer>();

builder.Services.AddHttpClient("ApiClient", httpClient =>
{
    httpClient.BaseAddress = new Uri("https://localhost:44301");
});

var app = builder.Build();

app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

await app.RunAsync();

//Authorize for Users who want to make orders
//Нормализация таблиц и построение БД
//Список "продуктов" с синхронизацией с бэка
//Раскрыть сущность "заказ" [Адрес etc.]
//Чо-то там с транзакциями Ж)