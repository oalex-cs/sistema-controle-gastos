import type { ConsultaTotais } from '../types'
import { formatarMoeda } from '../utils/formatters'

interface TotaisResumoProps {
  totais: ConsultaTotais | null
}

export function TotaisResumo({ totais }: TotaisResumoProps) {
  if (!totais) {
    return <p className="empty-state">Não foi possível carregar os totais.</p>
  }

  return (
    <>
      <div className="summary-grid">
        <article className="summary-card">
          <span>Receitas</span>
          <strong className="text-positive">
            {formatarMoeda(totais.totalGeral.totalReceitas)}
          </strong>
        </article>
        <article className="summary-card">
          <span>Despesas</span>
          <strong className="text-negative">
            {formatarMoeda(totais.totalGeral.totalDespesas)}
          </strong>
        </article>
        <article className="summary-card">
          <span>Saldo</span>
          <strong
            className={
              totais.totalGeral.saldo >= 0
                ? 'text-positive'
                : 'text-negative'
            }
          >
            {formatarMoeda(totais.totalGeral.saldo)}
          </strong>
        </article>
      </div>

      {totais.pessoas.length === 0 ? (
        <p className="empty-state">Nenhuma pessoa cadastrada.</p>
      ) : (
        <div className="table-wrapper">
          <table>
            <thead>
              <tr>
                <th>Pessoa</th>
                <th>Receitas</th>
                <th>Despesas</th>
                <th>Saldo</th>
              </tr>
            </thead>
            <tbody>
              {totais.pessoas.map((pessoa) => (
                <tr key={pessoa.pessoaId}>
                  <td>{pessoa.pessoaNome}</td>
                  <td>{formatarMoeda(pessoa.totalReceitas)}</td>
                  <td>{formatarMoeda(pessoa.totalDespesas)}</td>
                  <td>{formatarMoeda(pessoa.saldo)}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </>
  )
}