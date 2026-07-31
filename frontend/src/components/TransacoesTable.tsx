import type { Transacao } from '../types'
import { formatarMoeda, obterNomeTipo } from '../utils/formatters'

interface TransacoesTableProps {
  transacoes: Transacao[]
}

export function TransacoesTable({
  transacoes,
}: TransacoesTableProps) {
  if (transacoes.length === 0) {
    return <p className="empty-state">Nenhuma transação cadastrada.</p>
  }

  return (
    <div className="table-wrapper">
      <table>
        <thead>
          <tr>
            <th>Descrição</th>
            <th>Pessoa</th>
            <th>Tipo</th>
            <th>Valor</th>
          </tr>
        </thead>
        <tbody>
          {transacoes.map((transacao) => (
            <tr key={transacao.id}>
              <td>{transacao.descricao}</td>
              <td>{transacao.pessoaNome}</td>
              <td>
                <span
                  className={`badge badge--${
                    transacao.tipo === 2 ? 'receita' : 'despesa'
                  }`}
                >
                  {obterNomeTipo(transacao.tipo)}
                </span>
              </td>
              <td>{formatarMoeda(transacao.valor)}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  )
}