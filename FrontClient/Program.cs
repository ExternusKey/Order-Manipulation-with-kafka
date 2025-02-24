using ServicesManipulation.Controllers;
using ServicesManipulation.Data;
using ServicesManipulation.Messaging;
using ServicesManipulation.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddHostedService<ClientOrderUpdaterConsumer>();
builder.Services.AddSingleton<ConfirmationController>();
builder.Services.AddHttpClient<GpuModelService>(httpClient => httpClient.BaseAddress = new Uri("http://localhost:59291"));

var app = builder.Build();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

await app.RunAsync();

//Authorize for Users who want to make orders
//Нормализация таблиц и построение БД
//Список "продуктов" с синхронизацией с бэка
//Раскрыть сущность "заказ" [Адрес etc.]
//Чо-то там с транзакциями Ж)