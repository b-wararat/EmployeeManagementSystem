using BaseLibrary.Entities;
using Blazored.LocalStorage;
using Client;
using Client.ApplicationState;
using ClientLibrary.Helpers;
using ClientLibrary.Services.Contracts;
using ClientLibrary.Services.Implementations;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Popups;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddTransient<CustomHttpHandler>();
builder.Services.AddHttpClient("SystemApiClient", client => {
    client.BaseAddress = new Uri("https://localhost:7004/");
}).AddHttpMessageHandler<CustomHttpHandler>();

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7004/") });
builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<GetHttpClient>();
builder.Services.AddScoped<LocalStorageService>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddScoped<IUserAccountService, UserAccountService>();
//General Department / Department /Branch
builder.Services.AddScoped<IGenericService<GeneralDepartment>, GenericService<GeneralDepartment>>();
builder.Services.AddScoped<IGenericService<Department>, GenericService<Department>>();
builder.Services.AddScoped<IGenericService<Branch>, GenericService<Branch>>();
//Country / City / Town
builder.Services.AddScoped<IGenericService<Country>, GenericService<Country>>();
builder.Services.AddScoped<IGenericService<City>, GenericService<City>>();
builder.Services.AddScoped<IGenericService<Town>, GenericService<Town>>();
//Employee
builder.Services.AddScoped<IGenericService<Employee>, GenericService<Employee>>();

builder.Services.AddScoped<AllState>(); 

Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NMaF5cXmBCfEx3Rnxbf1x1ZFRGalhQTndWUiweQnxTdEBjWH5acXRRR2JdU0Z+Ww==");
builder.Services.AddSyncfusionBlazor();
builder.Services.AddScoped<SfDialogService>();


await builder.Build().RunAsync();
