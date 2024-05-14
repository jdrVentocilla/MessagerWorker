

namespace MessagerWorker.Models
{
    public class TransferEntity
    {
        public string Id { get; set; }
        public string NCuentaOrigen { get; set; }
        public string NCuentaDestino { get; set; }

        public string BancoDestino { get; set; }
        public string BancoOrigen { get; set; }
        public double Monto { get; set; }
        public DateTime FechaOperacion { get; set; }
        public string TMovimiento { get; set; }

       
    }
}
