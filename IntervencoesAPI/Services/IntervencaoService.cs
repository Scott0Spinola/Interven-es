using System.Data;
using IntervencoesAPI.Data;
using IntervencoesAPI.Dtos;
using IntervencoesAPI.Models;

using Microsoft.EntityFrameworkCore;

namespace IntervencoesAPI.Services;

public class IntervencaoService
{
    private readonly IntervencoesAPIContext _context;

    private readonly ILogger<IntervencaoService> _logger;

    public IntervencaoService(IntervencoesAPIContext context, ILogger<IntervencaoService> logger)
	{
		_context = context;
		_logger = logger;
	}

    public List<Intervencao> GetAll()
    {
        return _context.Intervencaos.OrderBy(i => i.Id).ToList();
    }

    	public async Task<PagedList<Intervencao>> GetAllPagedAsync(PageParameters pageParameters)
	{
		var query = _context.Intervencaos
			.AsNoTracking()
			.OrderBy(i => i.Id)
			.AsQueryable();

		return await PagedList<Intervencao>.CreateAsync(query, pageParameters.PageNumber, pageParameters.PageSize);
	}

    public Intervencao? GetByIdIntervencao(int id)
    {
        return _context.Intervencaos.FirstOrDefault(i => i.Id == id);
    }

    public async Task<Intervencao> CreateAsync(CreateIntervencao dto)
    {
        var intervencao = new Intervencao
        {
            ProcessoId = dto.ProcessoId,
            TarefaId = dto.TarefaId,
            Autor = dto.Autor,
            Visibilidade = dto.Visibilidade,
            Tipo = dto.Tipo,
            Estado = dto.Estado,
            HistoricoEstados = dto.HistoricoEstados,
            Prioridade = dto.Prioridade,
            DataRegisto = dto.DataRegisto,
            DataLimite = dto.DataLimite,
            DataConclusao = dto.DataConclusao,
            DataConfirmacao = dto.DataConfirmacao,
            DataInstalacao = dto.DataInstalacao,
            Tema = dto.Tema,
            AccaoRealizada = dto.AccaoRealizada,
            PrevisaoEsforco = dto.PrevisaoEsforco,
            EsforcoReal = dto.EsforcoReal,
            Referencia = dto.Referencia,
            Responsavel = dto.Responsavel,
            Coresponsavel = dto.Coresponsavel,
            Descricao = dto.Descricao,
            Notas = dto.Notas,
            Comentarios = dto.Comentarios,
            IntervencaoPaiId = dto.IntervencaoPaiId,
            EsforcoACobrar = dto.EsforcoACobrar,
            Valor = dto.Valor,
            DataCriacao = dto.DataCriacao,
            DataQualidade = dto.DataQualidade,
            UpdateUser = dto.UpdateUser,
            UpdateDate = dto.UpdateDate,
            Email = dto.Email,
            Codigo = dto.Codigo,
            DataInicio = dto.DataInicio,
            TarefaAgendadaId = dto.TarefaAgendadaId,
            DataInicioPrevista = dto.DataInicioPrevista,
            DataFimPrevista = dto.DataFimPrevista,
            Alerta = dto.Alerta,
            MotivoAlerta = dto.MotivoAlerta,
        };

        _context.Intervencaos.Add(intervencao);
        await _context.SaveChangesAsync();
        return intervencao;
    }

    public async Task<Intervencao?> UpdateAsync(int id, UpdateIntervencao dto)
    {
        var intervencao = _context.Intervencaos.FirstOrDefault(i => i.Id == id);

        if (intervencao is null)
        {
            return null;
        }

        intervencao.ProcessoId = dto.ProcessoId;
        intervencao.TarefaId = dto.TarefaId;
        intervencao.Autor = dto.Autor;
        intervencao.Visibilidade = dto.Visibilidade;
        intervencao.Tipo = dto.Tipo;
        intervencao.Estado = dto.Estado;
        intervencao.HistoricoEstados = dto.HistoricoEstados;
        intervencao.Prioridade = dto.Prioridade;
        intervencao.DataRegisto = dto.DataRegisto;
        intervencao.DataLimite = dto.DataLimite;
        intervencao.DataConclusao = dto.DataConclusao;
        intervencao.DataConfirmacao = dto.DataConfirmacao;
        intervencao.DataInstalacao = dto.DataInstalacao;
        intervencao.Tema = dto.Tema;
        intervencao.AccaoRealizada = dto.AccaoRealizada;
        intervencao.PrevisaoEsforco = dto.PrevisaoEsforco;
        intervencao.EsforcoReal = dto.EsforcoReal;
        intervencao.Referencia = dto.Referencia;
        intervencao.Responsavel = dto.Responsavel;
        intervencao.Coresponsavel = dto.Coresponsavel;
        intervencao.Descricao = dto.Descricao;
        intervencao.Notas = dto.Notas;
        intervencao.Comentarios = dto.Comentarios;
        intervencao.IntervencaoPaiId = dto.IntervencaoPaiId;
        intervencao.EsforcoACobrar = dto.EsforcoACobrar;
        intervencao.Valor = dto.Valor;
        intervencao.DataCriacao = dto.DataCriacao;
        intervencao.DataQualidade = dto.DataQualidade;
        intervencao.UpdateUser = dto.UpdateUser;
        intervencao.UpdateDate = dto.UpdateDate;
        intervencao.Email = dto.Email;
        intervencao.Codigo = dto.Codigo;
        intervencao.DataInicio = dto.DataInicio;
        intervencao.TarefaAgendadaId = dto.TarefaAgendadaId;
        intervencao.DataInicioPrevista = dto.DataInicioPrevista;
        intervencao.DataFimPrevista = dto.DataFimPrevista;
        intervencao.Alerta = dto.Alerta;
        intervencao.MotivoAlerta = dto.MotivoAlerta;

        await _context.SaveChangesAsync();
        return intervencao;
    }

    public bool Delete(int id)
    {
        var intervencao = _context.Intervencaos.FirstOrDefault(i => i.Id == id);
        if (intervencao is null)
        {
            return false;
        }

        _context.Intervencaos.Remove(intervencao);
        _context.SaveChanges();
        return true;
    }
}