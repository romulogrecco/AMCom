using Questao2;
using Refit;

public class Program
{
    public static async Task Main()
    {
        string teamName = "Paris Saint-Germain";
        int year = 2013;
        int totalGoals = await getTotalScoredGoals(teamName, year.ToString());

        Console.WriteLine("Team "+ teamName +" scored "+ totalGoals.ToString() + " goals in "+ year);

        teamName = "Chelsea";
        year = 2014;
        totalGoals = await getTotalScoredGoals(teamName, year.ToString());

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        // Output expected:
        // Team Paris Saint - Germain scored 109 goals in 2013
        // Team Chelsea scored 92 goals in 2014
    }


    // Utilizando a biblioteca Refit para realizar chamadas
    public static async Task<int> getTotalScoredGoals(string team, string year)
    {
        var cliente = RestService.For<IGoalsApiService>("https://jsonmock.hackerrank.com/api");

        var respostaApi = await cliente.GetResultAsync(year, team);

        if (respostaApi != null)
        {
            var gols = respostaApi.ObterQuantidadeGolsPaginaAtual();

            for (int i = 2; i <= respostaApi.total_pages; i++)
            {
                var respostaApiPaginaAtual = await cliente.GetResultAsync(year, team, i);

                gols += respostaApiPaginaAtual.ObterQuantidadeGolsPaginaAtual();
            }

            return gols;
        }

        return 0;
    }

    public interface IGoalsApiService
    {
        [Get("/football_matches?year={ano}&team1={nome}&page={pagina}")]
        Task<ResponseRank> GetResultAsync(string ano, string nome, int pagina = 1);
    }

}