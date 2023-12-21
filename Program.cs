var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddIniFile("appcompany.ini");
builder.Configuration.AddJsonFile("appcompany.json");
builder.Configuration.AddXmlFile("appcompany.xml");

builder.Configuration.AddJsonFile("personalInfo.json");

builder.Services.AddRazorPages();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapGet("/", async (HttpContext context) =>
{
    await Result(context, app.Configuration);
});
app.Run();

async Task Result(HttpContext context, IConfiguration configuration)
{
    var companies = configuration.GetSection("Companies").GetChildren();
    var companyWithMostEmployees = companies.OrderByDescending(c => int.Parse(c["Employees"])).First();

    await context.Response.WriteAsync($"Task 1\n");
    await context.Response.WriteAsync($"Company with most employeers: {companyWithMostEmployees.Key}\n");
    await context.Response.WriteAsync($"Employeers count: {companyWithMostEmployees["Employees"]}\n");

    await context.Response.WriteAsync($"\nTask 2\n");
    var personalInfo = configuration.GetSection("Information:You");
    await context.Response.WriteAsync($"Your name: {personalInfo["Name"]}\n");
    await context.Response.WriteAsync($"Your age: {personalInfo["Age"]}\n");
    await context.Response.WriteAsync($"Your city: {personalInfo["City"]}\n");
}
