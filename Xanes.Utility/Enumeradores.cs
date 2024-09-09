namespace Xanes.Utility;
public static class Enumeradores
{

    [Flags]
    public enum BancoConsecutivoPor : int
    {
        Categoria = 1,
        CategoriaMensual = 2,
        CategoriaAnual = 4,
        Tipo = 8,
        TipoMensual = 16,
        TipoAnual = 32
    }

    [Flags]
    public enum ConsecutivoTipo : int
    {
        Temporal = 1,
        Perpetuo = 2
    }

    [Flags]

    public enum mexBankTransactionStages : int
    {
        Draft = 128,
        Aproval = 512
    }

    [Flags]
    public enum mexJournalEntryStages : int
    {
        New = 64,
        Draft = 128,
        Aproval = 256,
        DisApprove = 512,
        Void = 1024
    }

    public enum mexBankDetailType : int
    {
        MainAccount = 1,
        TaxWithholding = 2,
        Differential = 4,
        Other = 8,
        AutomaticCounterPart = 16,
        TransferCommission = 32
    }

    public enum mexAccountMovementType : short
    {
        Debit = 1,
        Credit = 2
    }

    public enum mexBankObjects : int
    {
        Transaction = 5,
    }
    public enum mexJournalObjects : int
    {
        JournalEntry = 3,
    }


    [Flags]
    public enum TransaccionBcoTipo : int
    {
        Pago = 1,
        Deposito = 2,
        NotaDebito = 4,
        NotaCredito = 8,
        Transferencia = 16
    }

    [Flags]
    public enum TransaccionBcoPagoSubtipo : int
    {
        Cheque = 1,
        Transferencia = 2,
        MesaCambio = 4
    }

    [Flags]
    public enum TransaccionBcoDepositoSubtipo : int
    {
        Deposito = 1
    }

    [Flags]
    public enum TransaccionBcoNotaDebitoSubtipo : int
    {
        NotaDebito = 1,
        NotaDebitoDiferencia = 2
    }

    [Flags]
    public enum TransaccionBcoNotaCreditoSubtipo : int
    {
        NotaCredito = 1,
        NotaCreditoDiferencia = 2
    }

    [Flags]
    public enum TransaccionBcoTransferenciaSubtipo : int
    {
        Transferencia = 1,
        TransferenciaDebito = 2,
        TransferenciaCredito = 4
    }
}

