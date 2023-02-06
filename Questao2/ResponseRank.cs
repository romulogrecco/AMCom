namespace Questao2;

public class ResponseRank
{  
    public int page { get; set; }
    public int per_page { get; set; }
    public int total { get; set; }
    public int total_pages { get; set; }
    public List<ResponseTeam> data { get; set; }

    public int ObterQuantidadeGolsPaginaAtual()
    {
        return data.Sum(x => Convert.ToInt32(x.team1goals));
    }
}

public class ResponseTeam
{
    public string competition { get; set; }
    public int year { get; set; }
    public string round { get; set; }
    public string team1 { get; set; }
    public string team2 { get; set; }
    public string team1goals { get; set; }
    public string team2goals { get; set; }
}