import { useState, type FormEvent } from 'react'
import type { CriarPessoaRequest } from '../types'

interface PessoaFormProps {
  enviando: boolean
  onSubmit: (request: CriarPessoaRequest) => Promise<void>
}

export function PessoaForm({
  enviando,
  onSubmit,
}: PessoaFormProps) {
  const [nome, setNome] = useState('')
  const [idade, setIdade] = useState('')

  async function handleSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault()

    const idadeNumerica = Number(idade)

    if (!nome.trim() || !Number.isInteger(idadeNumerica)) {
      return
    }

    try {
      await onSubmit({
        nome: nome.trim(),
        idade: idadeNumerica,
      })

      setNome('')
      setIdade('')
    } catch {
      // O componente pai já apresenta a mensagem retornada pela API.
    }
  }

  return (
    <form className="form" onSubmit={handleSubmit}>
      <div className="form__field form__field--wide">
        <label htmlFor="nome">Nome</label>
        <input
          id="nome"
          value={nome}
          onChange={(event) => setNome(event.target.value)}
          maxLength={100}
          placeholder="Nome da pessoa"
          required
        />
      </div>

      <div className="form__field">
        <label htmlFor="idade">Idade</label>
        <input
          id="idade"
          type="number"
          value={idade}
          onChange={(event) => setIdade(event.target.value)}
          min={0}
          max={130}
          placeholder="0"
          required
        />
      </div>

      <button className="button button--primary" disabled={enviando}>
        {enviando ? 'Cadastrando...' : 'Cadastrar pessoa'}
      </button>
    </form>
  )
}