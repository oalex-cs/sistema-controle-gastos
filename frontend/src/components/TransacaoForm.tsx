import { useState, type FormEvent } from 'react'
import {
  TipoTransacao,
  type CriarTransacaoRequest,
  type Pessoa,
} from '../types'

interface TransacaoFormProps {
  pessoas: Pessoa[]
  enviando: boolean
  onSubmit: (request: CriarTransacaoRequest) => Promise<void>
}

export function TransacaoForm({
  pessoas,
  enviando,
  onSubmit,
}: TransacaoFormProps) {
  const [descricao, setDescricao] = useState('')
  const [valor, setValor] = useState('')
  const [tipo, setTipo] = useState<TipoTransacao>(
    TipoTransacao.Despesa,
  )
  const [pessoaId, setPessoaId] = useState('')

  async function handleSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault()

    const valorNumerico = Number(valor.replace(',', '.'))

    if (!descricao.trim() || valorNumerico <= 0 || !pessoaId) {
      return
    }

    try {
      await onSubmit({
        descricao: descricao.trim(),
        valor: valorNumerico,
        tipo,
        pessoaId,
      })

      setDescricao('')
      setValor('')
      setTipo(TipoTransacao.Despesa)
      setPessoaId('')
    } catch {
      // O componente pai já apresenta a mensagem retornada pela API.
    }
  }

  const semPessoas = pessoas.length === 0

  return (
    <form className="form" onSubmit={handleSubmit}>
      <div className="form__field form__field--wide">
        <label htmlFor="descricao">Descrição</label>
        <input
          id="descricao"
          value={descricao}
          onChange={(event) => setDescricao(event.target.value)}
          maxLength={200}
          placeholder="Ex.: Mercado"
          required
        />
      </div>

      <div className="form__field">
        <label htmlFor="valor">Valor</label>
        <input
          id="valor"
          inputMode="decimal"
          value={valor}
          onChange={(event) => setValor(event.target.value)}
          placeholder="0,00"
          required
        />
      </div>

      <div className="form__field">
        <label htmlFor="tipo">Tipo</label>
        <select
          id="tipo"
          value={tipo}
          onChange={(event) =>
            setTipo(Number(event.target.value) as TipoTransacao)
          }
        >
          <option value={TipoTransacao.Despesa}>Despesa</option>
          <option value={TipoTransacao.Receita}>Receita</option>
        </select>
      </div>

      <div className="form__field form__field--wide">
        <label htmlFor="pessoa">Pessoa</label>
        <select
          id="pessoa"
          value={pessoaId}
          onChange={(event) => setPessoaId(event.target.value)}
          disabled={semPessoas}
          required
        >
          <option value="">
            {semPessoas
              ? 'Cadastre uma pessoa primeiro'
              : 'Selecione uma pessoa'}
          </option>
          {pessoas.map((pessoa) => (
            <option key={pessoa.id} value={pessoa.id}>
              {pessoa.nome} — {pessoa.idade} anos
            </option>
          ))}
        </select>
      </div>

      <button
        className="button button--primary"
        disabled={enviando || semPessoas}
      >
        {enviando ? 'Cadastrando...' : 'Cadastrar transação'}
      </button>
    </form>
  )
}