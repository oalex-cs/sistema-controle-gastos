interface FeedbackProps {
  tipo: 'sucesso' | 'erro'
  mensagem: string
  onClose: () => void
}

export function Feedback({
  tipo,
  mensagem,
  onClose,
}: FeedbackProps) {
  return (
    <div className={`feedback feedback--${tipo}`} role="alert">
      <span>{mensagem}</span>
      <button
        type="button"
        className="feedback__close"
        onClick={onClose}
        aria-label="Fechar mensagem"
      >
        ×
      </button>
    </div>
  )
}