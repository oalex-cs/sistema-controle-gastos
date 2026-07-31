import { apiRequest } from './api'
import type { CriarPessoaRequest, Pessoa } from '../types'

export function listarPessoas(): Promise<Pessoa[]> {
  return apiRequest<Pessoa[]>('/api/pessoas')
}

export function criarPessoa(
  request: CriarPessoaRequest,
): Promise<Pessoa> {
  return apiRequest<Pessoa>('/api/pessoas', {
    method: 'POST',
    body: JSON.stringify(request),
  })
}

export function excluirPessoa(id: string): Promise<void> {
  return apiRequest<void>(`/api/pessoas/${id}`, {
    method: 'DELETE',
  })
}