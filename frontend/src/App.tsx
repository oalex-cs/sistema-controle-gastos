import { useCallback, useEffect, useState } from 'react'
import { Feedback } from './components/Feedback'
import { PessoaForm } from './components/PessoaForm'
import { PessoasTable } from './components/PessoaTable'
import { TotaisResumo } from './components/TotaisResumo'
import { TransacaoForm } from './components/TransacaoForm'
import { TransacoesTable } from './components/TransacoesTable'
import { obterMensagemErro } from './services/api'
import {
  criarPessoa,
  excluirPessoa,
  listarPessoas,
} from './services/pessoas'

import { consultarTotais } from './services/totais'

import {
  criarTransacao,
  listarTransacoes,
} from './services/transacoes'

import type {
  ConsultaTotais,
  CriarPessoaRequest,
  CriarTransacaoRequest,
  Pessoa,
  Transacao,
} from './types'
import './styles.css'

type Aba = 'pessoas' | 'transacoes' | 'totais'

interface FeedbackState {
  tipo: 'sucesso' | 'erro'
  mensagem: string
}

export default function App() {
  const [abaAtiva, setAbaAtiva] = useState<Aba>('pessoas')
  const [pessoas, setPessoas] = useState<Pessoa[]>([])
  const [transacoes, setTransacoes] = useState<Transacao[]>([])
  const [totais, setTotais] = useState<ConsultaTotais | null>(null)
  const [carregando, setCarregando] = useState(true)
  const [salvandoPessoa, setSalvandoPessoa] = useState(false)
  const [salvandoTransacao, setSalvandoTransacao] = useState(false)
  const [excluindoPessoaId, setExcluindoPessoaId] = useState<string | null>(null)
  const [feedback, setFeedback] = useState<FeedbackState | null>(null)

  const carregarDados = useCallback(async () => {
    try {
      const [pessoasResponse, transacoesResponse, totaisResponse] =
        await Promise.all([
          listarPessoas(),
          listarTransacoes(),
          consultarTotais(),
        ])

      setPessoas(pessoasResponse)
      setTransacoes(transacoesResponse)
      setTotais(totaisResponse)
    } catch (error) {
      setFeedback({
        tipo: 'erro',
        mensagem: obterMensagemErro(error),
      })
    } finally {
      setCarregando(false)
    }
  }, [])

  useEffect(() => {
    void carregarDados()
  }, [carregarDados])

  async function handleCriarPessoa(request: CriarPessoaRequest) {
    setSalvandoPessoa(true)

    try {
      await criarPessoa(request)
      await carregarDados()
      setFeedback({
        tipo: 'sucesso',
        mensagem: 'Pessoa cadastrada com sucesso.',
      })
    } catch (error) {
      setFeedback({
        tipo: 'erro',
        mensagem: obterMensagemErro(error),
      })
      throw error
    } finally {
      setSalvandoPessoa(false)
    }
  }

  async function handleExcluirPessoa(pessoa: Pessoa) {
    const confirmou = window.confirm(
      `Excluir ${pessoa.nome} e todas as suas transações?`,
    )

    if (!confirmou) {
      return
    }

    setExcluindoPessoaId(pessoa.id)

    try {
      await excluirPessoa(pessoa.id)
      await carregarDados()
      setFeedback({
        tipo: 'sucesso',
        mensagem: 'Pessoa excluída com sucesso.',
      })
    } catch (error) {
      setFeedback({
        tipo: 'erro',
        mensagem: obterMensagemErro(error),
      })
    } finally {
      setExcluindoPessoaId(null)
    }
  }

  async function handleCriarTransacao(
    request: CriarTransacaoRequest,
  ) {
    setSalvandoTransacao(true)

    try {
      await criarTransacao(request)
      await carregarDados()
      setFeedback({
        tipo: 'sucesso',
        mensagem: 'Transação cadastrada com sucesso.',
      })
    } catch (error) {
      setFeedback({
        tipo: 'erro',
        mensagem: obterMensagemErro(error),
      })
      throw error
    } finally {
      setSalvandoTransacao(false)
    }
  }

  return (
    <div className="app-shell">
      <header className="hero">
        <div>
          <span className="eyebrow">Controle financeiro residencial</span>
          <h1>Gerenciador Financeiro</h1>
          <p>
            Cadastre pessoas e movimentações e acompanhe os totais em um só lugar.
          </p>
        </div>

        <button
          type="button"
          className="button button--secondary"
          onClick={() => {
            setCarregando(true)
            void carregarDados()
          }}
        >
          Atualizar dados
        </button>
      </header>

      {feedback && (
        <Feedback
          tipo={feedback.tipo}
          mensagem={feedback.mensagem}
          onClose={() => setFeedback(null)}
        />
      )}

      <nav className="tabs" aria-label="Seções do sistema">
        <button
          className={abaAtiva === 'pessoas' ? 'tab tab--active' : 'tab'}
          onClick={() => setAbaAtiva('pessoas')}
        >
          Pessoas
        </button>
        <button
          className={abaAtiva === 'transacoes' ? 'tab tab--active' : 'tab'}
          onClick={() => setAbaAtiva('transacoes')}
        >
          Transações
        </button>
        <button
          className={abaAtiva === 'totais' ? 'tab tab--active' : 'tab'}
          onClick={() => setAbaAtiva('totais')}
        >
          Totais
        </button>
      </nav>

      <main>
        {carregando ? (
          <section className="panel loading-state">Carregando dados...</section>
        ) : (
          <>
            {abaAtiva === 'pessoas' && (
              <section className="panel">
                <div className="section-heading">
                  <div>
                    <span className="eyebrow">Cadastro</span>
                    <h2>Pessoas</h2>
                  </div>
                  <span className="counter">{pessoas.length} cadastradas</span>
                </div>

                <PessoaForm
                  enviando={salvandoPessoa}
                  onSubmit={handleCriarPessoa}
                />

                <PessoasTable
                  pessoas={pessoas}
                  excluindoId={excluindoPessoaId}
                  onExcluir={handleExcluirPessoa}
                />
              </section>
            )}

            {abaAtiva === 'transacoes' && (
              <section className="panel">
                <div className="section-heading">
                  <div>
                    <span className="eyebrow">Movimentações</span>
                    <h2>Transações</h2>
                  </div>
                  <span className="counter">
                    {transacoes.length} cadastradas
                  </span>
                </div>

                <TransacaoForm
                  pessoas={pessoas}
                  enviando={salvandoTransacao}
                  onSubmit={handleCriarTransacao}
                />

                <TransacoesTable transacoes={transacoes} />
              </section>
            )}

            {abaAtiva === 'totais' && (
              <section className="panel">
                <div className="section-heading">
                  <div>
                    <span className="eyebrow">Consolidado</span>
                    <h2>Totais financeiros</h2>
                  </div>
                </div>

                <TotaisResumo totais={totais} />
              </section>
            )}
          </>
        )}
      </main>
    </div>
  )
}