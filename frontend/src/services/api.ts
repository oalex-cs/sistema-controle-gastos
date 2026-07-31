/**
 * Representa o formato padronizado de erros retornados pela API.
 * traceId auxilia o suporte e errors contém falhas de validação por campo.
 */
export interface ProblemDetails {
  type?: string
  title?: string
  status?: number
  detail?: string
  instance?: string
  traceId?: string
  errors?: Record<string, string[]>
}

/**
 * Preserva o status HTTP e os detalhes retornados pela API.
 */
export class ApiError extends Error {
  public readonly status: number
  public readonly problemDetails: ProblemDetails | null

  public constructor(
    status: number,
    problemDetails: ProblemDetails | null,
  ) {
    // Prioriza a mensagem mais específica disponível.
    const message =
      problemDetails?.detail ??
      problemDetails?.title ??
      `A API retornou o status ${status}.`

    super(message)

    this.name = 'ApiError'
    this.status = status
    this.problemDetails = problemDetails
  }
}

// Remove a barra final para evitar URLs com barras duplicadas.
const apiBaseUrl = import.meta.env.VITE_API_URL?.replace(/\/$/, '')

// Interrompe a inicialização quando a API não estiver configurada.
if (!apiBaseUrl) {
  throw new Error(
    'A variável de ambiente VITE_API_URL não foi configurada.',
  )
}

/**
 * Centraliza chamadas HTTP e padroniza headers, respostas e erros.
 */
export async function apiRequest<T>(
  path: string,
  options: RequestInit = {},
): Promise<T> {
  const headers = new Headers(options.headers)

  headers.set('Accept', 'application/json')

  if (options.body && !headers.has('Content-Type')) {
    headers.set('Content-Type', 'application/json')
  }

  const response = await fetch(`${apiBaseUrl}${path}`, {
    ...options,
    headers,
  })

  if (!response.ok) {
    const contentType = response.headers.get('content-type')
    let problemDetails: ProblemDetails | null = null

    // Um corpo inválido não substitui o erro HTTP original.
    if (contentType?.includes('json')) {
      try {
        problemDetails =
          (await response.json()) as ProblemDetails
      } catch {
        problemDetails = null
      }
    }

    throw new ApiError(response.status, problemDetails)
  }

  // Respostas 204 não possuem corpo para desserialização.
  if (response.status === 204) {
    return undefined as T
  }

  return (await response.json()) as T
}

export function obterMensagemErro(error: unknown): string {
  if (!(error instanceof ApiError)) {
    return error instanceof Error
      ? error.message
      : 'Ocorreu um erro inesperado.'
  }

  const validationMessages = Object.values(
    error.problemDetails?.errors ?? {},
  ).flat()

  if (validationMessages.length > 0) {
    return validationMessages.join(' ')
  }

  return error.message
}