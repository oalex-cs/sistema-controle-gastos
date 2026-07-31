export interface Pessoa {
  id: string
  nome: string
  idade: number
}

export interface CriarPessoaRequest {
  nome: string
  idade: number
}

export enum TipoTransacao {
  Despesa = 1,
  Receita = 2,
}

export interface Transacao {
  id: string
  descricao: string
  valor: number
  tipo: TipoTransacao
  pessoaId: string
  pessoaNome: string
}

export interface CriarTransacaoRequest {
  descricao: string
  valor: number
  tipo: TipoTransacao
  pessoaId: string
}

export interface TotalPessoa {
  pessoaId: string
  pessoaNome: string
  totalReceitas: number
  totalDespesas: number
  saldo: number
}

export interface TotalGeral {
  totalReceitas: number
  totalDespesas: number
  saldo: number
}

export interface ConsultaTotais {
  pessoas: TotalPessoa[]
  totalGeral: TotalGeral
}