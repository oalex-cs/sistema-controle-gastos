import { TipoTransacao } from '../types'

const currencyFormatter = new Intl.NumberFormat('pt-BR', {
  style: 'currency',
  currency: 'BRL',
})

export function formatarMoeda(valor: number): string {
  return currencyFormatter.format(valor)
}

export function obterNomeTipo(tipo: TipoTransacao): string {
  return tipo === TipoTransacao.Receita ? 'Receita' : 'Despesa'
}