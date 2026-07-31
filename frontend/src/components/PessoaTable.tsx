import type { Pessoa } from '../types'

interface PessoasTableProps {
  pessoas: Pessoa[]
  excluindoId: string | null
  onExcluir: (pessoa: Pessoa) => Promise<void>
}

export function PessoasTable({
  pessoas,
  excluindoId,
  onExcluir,
}: PessoasTableProps) {
  if (pessoas.length === 0) {
    return <p className="empty-state">Nenhuma pessoa cadastrada.</p>
  }

  return (
    <div className="table-wrapper">
      <table>
        <thead>
          <tr>
            <th>Nome</th>
            <th>Idade</th>
            <th className="table__actions">Ações</th>
          </tr>
        </thead>
        <tbody>
          {pessoas.map((pessoa) => (
            <tr key={pessoa.id}>
              <td>{pessoa.nome}</td>
              <td>{pessoa.idade} anos</td>
              <td className="table__actions">
                <button
                  type="button"
                  className="button button--danger button--small"
                  disabled={excluindoId === pessoa.id}
                  onClick={() => void onExcluir(pessoa)}
                >
                  {excluindoId === pessoa.id ? 'Excluindo...' : 'Excluir'}
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  )
}