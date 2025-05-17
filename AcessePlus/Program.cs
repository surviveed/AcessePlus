//using AcessePlus.Modelo;
//using System.Diagnostics.Tracing;
//using System.Security.AccessControl;

////testes falta concluir

//try
//{
//    #region Pais 
//    var pais = new Pais()
//    {
//        CodigoIbge = 1,
//        Descricao = "Brasil"
//    };

//    new AcessePlus.Negocio.Pais().Salvar(pais);

//    var paisCadastrada= new AcessePlus.Negocio.Pais().BuscarPorId(1);

//    var paisesCadastrados = new AcessePlus.Negocio.Pais().BuscarTodos();

//    paisCadastrada.Descricao = "Argentina";
//    paisCadastrada.CodigoIbge = 2;

//    new AcessePlus.Negocio.Pais().Salvar(paisCadastrada);
//    #endregion
//    #region Uf
//    var uf = new Uf()
//    {
//        CodigoIbge = 1,
//        Descricao = "Rio Grande do Sul",
//    };

//    uf.Pais.Id = paisCadastrada.Id;

//    new AcessePlus.Negocio.Uf().Salvar(uf);

//    var ufCadastrada = new AcessePlus.Negocio.Uf().BuscarPorId(1);

//    var ufsCadastradas = new AcessePlus.Negocio.Uf().BuscarTodos();

//    ufCadastrada.CodigoIbge = 2;
//    ufCadastrada.Descricao = "Teste";
//    ufCadastrada.Pais.Id = 2;

//    new AcessePlus.Negocio.Uf().Salvar(ufCadastrada);
//    #endregion
//    #region Cidade
//    var cidade = new Cidade()
//    {
//        CodigoIbge = 1,
//        Descricao = "Rio Grande do Sul",
//    };

//    cidade.Uf.Id = ufCadastrada.Id;

//    new AcessePlus.Negocio.Cidade().Salvar(cidade);

//    var cidadeCadastrada = new AcessePlus.Negocio.Cidade().BuscarPorId(1);

//    var cidadesCadastradas = new AcessePlus.Negocio.Cidade().BuscarTodos();

//    cidadeCadastrada.CodigoIbge = 2;
//    cidadeCadastrada.Descricao = "Teste";
//    cidadeCadastrada.Uf.Id = 2;

//    new AcessePlus.Negocio.Cidade().Salvar(cidadeCadastrada);
//    #endregion
//    #region Local
//    var local = new Local()
//    {
//        Nome = "Local Exemplo",
//        Capacidade=1,
//        Endereco="Rua 123",
//        Observacoes="Exemplo Observação"
//    };

//    local.Cidade.Id = cidadeCadastrada.Id;

//    new AcessePlus.Negocio.Local().Salvar(local);

//    var localCadastrado = new AcessePlus.Negocio.Local().BuscarPorId(1);

//    var locaisCadastrados= new AcessePlus.Negocio.Local().BuscarTodos();

//    localCadastrado.Nome = "Local Exemplo alterado";
//    localCadastrado.Capacidade = 2;
//    localCadastrado.Endereco= "Rua Alterada";
//    localCadastrado.Observacoes="Exemplo observacao alterado";
//    localCadastrado.Cidade.Id = 2;

//    new AcessePlus.Negocio.Local().Salvar(localCadastrado);
//    #endregion
//    #region Tipo Evento
//    var tipoEvento = new TipoEvento()
//    {
//        Descricao = "Tipo Exemplo",
//    };

//    new AcessePlus.Negocio.TipoEvento().Salvar(tipoEvento);

//    var tipoEventoCadastrado = new AcessePlus.Negocio.TipoEvento().BuscarPorId(1);

//    var tiposEventosCadastrados = new AcessePlus.Negocio.TipoEvento().BuscarTodos();

//    tipoEventoCadastrado.Descricao = "Tipo Exemplo Alterado";

//    new AcessePlus.Negocio.TipoEvento().Salvar(tipoEventoCadastrado);
//    #endregion
//    #region Evento
//    var evento = new Evento()
//    {
//        Nome = "Evento Exemplo",
//        Descricao="Descricao do evento exemplo",
//    };

//    evento.Local = localCadastrado;
//    evento.TipoEvento = tipoEventoCadastrado;

//    new AcessePlus.Negocio.Evento().Salvar(evento);

//    var eventoCadstrado = new AcessePlus.Negocio.Evento().BuscarPorId(1);

//    var eventosCadastrados = new AcessePlus.Negocio.Evento().BuscarTodos();

//    eventoCadstrado.Nome = "Evento Exemplo Alterado";
//    eventoCadstrado.Descricao= "Descricao do Evento Exemplo Alterado";
//    eventoCadstrado.Local.Id = 2;
//    eventoCadstrado.TipoEvento.Id = 2;

//    new AcessePlus.Negocio.Evento().Salvar(eventoCadstrado);
//    #endregion
//    #region Avaliacao
//    var avaliacao = new Avaliacao()
//    {
//        Comentario = "Comentario exemplo",
//        TipoAcessibilidade_Enum = Avaliacao.eTipoAcessibilidade.Auditiva,
//        Tipo_Enum=Avaliacao.eTipo.Positiva
//    };

//    avaliacao.Local = localCadastrado;

//    new AcessePlus.Negocio.Avaliacao().Salvar(avaliacao);

//    var avaliacaoCadastrada = new AcessePlus.Negocio.Avaliacao().BuscarPorId(1);

//    var avaliacaoesCadastradas = new AcessePlus.Negocio.Avaliacao().BuscarTodos();

//    avaliacaoCadastrada.Comentario = "Comentario Exemplo alterado";
//    avaliacaoCadastrada.TipoAcessibilidade_Enum =Avaliacao.eTipoAcessibilidade.Locomotiva;
//    avaliacaoCadastrada.Tipo_Enum =Avaliacao.eTipo.Negativa;
//    avaliacaoCadastrada.Local.Id = 2;

//    new AcessePlus.Negocio.Avaliacao().Salvar(avaliacaoCadastrada);
//    #endregion
//}
//catch (Exception ex)
//{
//    Console.WriteLine(ex.Message+" - "+ex.StackTrace);
//}


//Console.ReadKey();

using Microsoft.AspNetCore.Mvc.Razor;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    options.EnableEndpointRouting = false; // Disables endpoint-based routing
});

// Configure custom view locations
builder.Services.Configure<RazorViewEngineOptions>(options =>
{
    options.ViewLocationFormats.Clear();
    options.ViewLocationFormats.Add("/Views/{1}/{0}.cshtml");
    options.ViewLocationFormats.Add("/Views/Site/{1}/{0}.cshtml");
    options.ViewLocationFormats.Add("/Views/Site/Shared/{0}.cshtml");
    options.ViewLocationFormats.Add("/Views/Admin/{1}/{0}.cshtml");
    options.ViewLocationFormats.Add("/Views/Admin/Shared/{0}.cshtml");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();