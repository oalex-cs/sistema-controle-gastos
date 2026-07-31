import { apiRequest } from './api'
import type { ConsultaTotais } from '../types'

export function consultarTotais(): Promise<ConsultaTotais> {
  return apiRequest<ConsultaTotais>('/api/totais')
}