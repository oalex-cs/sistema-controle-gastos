import { apiRequest } from './api'
import type {
  CriarTransacaoRequest,
  Transacao,
} from '../types'

export function listarTransacoes(): Promise<Transacao[]> {
  return apiRequest<Transacao[]>('/api/transacoes')
}

export function criarTransacao(
  request: CriarTransacaoRequest,
): Promise<Transacao> {
  return apiRequest<Transacao>('/api/transacoes', {
    method: 'POST',
    body: JSON.stringify(request),
  })
}