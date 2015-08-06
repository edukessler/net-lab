using Imposto.Core.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Imposto.Core.Domain;

namespace TesteImposto
{
    public partial class FormImposto : Form
    {        
        public FormImposto()
        {
            InitializeComponent();
            LimparTela();

            cboEstadoOrigem.DataSource = new string[] { "AC", "AL", "AP", "AM", "BA", "CE", "DF", "ES", "GO", "MA", "MT", "MS", "MG", "PA", "PB", "PR", "PE", "PI", "RJ", "RN", "RS", "RO", "RR", "SC", "SP", "SE", "TO" };
            cboEstadoDestino.DataSource = new string[] { "AC", "AL", "AP", "AM", "BA", "CE", "DF", "ES", "GO", "MA", "MT", "MS", "MG", "PA", "PB", "PR", "PE", "PI", "RJ", "RN", "RS", "RO", "RR", "SC", "SP", "SE", "TO" };
        }

        private void LimparTela()
        {
            textBoxNomeCliente.Clear();

            dataGridViewPedidos.DataSource = null;
            dataGridViewPedidos.AutoGenerateColumns = true;                       
            dataGridViewPedidos.DataSource = GetTablePedidos();
            dataGridViewPedidos.Columns["Valor"].DefaultCellStyle.Format = "C";

            ResizeColumns();
        }

        private void ResizeColumns()
        {
            double mediaWidth = dataGridViewPedidos.Width / dataGridViewPedidos.Columns.GetColumnCount(DataGridViewElementStates.Visible);

            for (int i = dataGridViewPedidos.Columns.Count - 1; i >= 0; i--)
            {
                var coluna = dataGridViewPedidos.Columns[i];
                coluna.Width = Convert.ToInt32(mediaWidth);
            }   
        }

        private object GetTablePedidos()
        {
            DataTable table = new DataTable("pedidos");
            table.Columns.Add(new DataColumn("Nome do produto", typeof(string)));
            table.Columns.Add(new DataColumn("Codigo do produto", typeof(string)));

            DataColumn valor = new DataColumn("Valor", typeof(decimal));
            valor.DefaultValue = 0;                       
            table.Columns.Add(valor);

            DataColumn brinde = new DataColumn("Brinde", typeof(bool));
            brinde.DefaultValue = false;
            table.Columns.Add(brinde);
            
            return table;
        }

        private void buttonGerarNotaFiscal_Click(object sender, EventArgs e)
        {
            Pedido pedido = new Pedido();
            NotaFiscalService service = new NotaFiscalService();
            
            pedido.EstadoOrigem = cboEstadoOrigem.Text;
            pedido.EstadoDestino = cboEstadoDestino.Text;
            pedido.NomeCliente = textBoxNomeCliente.Text;

            DataTable table = (DataTable)dataGridViewPedidos.DataSource;
            foreach (DataRow row in table.Rows)
            {
                pedido.ItensDoPedido.Add(
                    new PedidoItem()
                    {
                        Brinde = Convert.ToBoolean(row["Brinde"]),
                        CodigoProduto =  row["Codigo do produto"].ToString(),
                        NomeProduto = row["Nome do produto"].ToString(),
                        ValorItemPedido = Convert.ToDouble(row["Valor"].ToString())            
                    });
            }

            service.GerarNotaFiscal(pedido);

            MessageBox.Show("Operação efetuada com sucesso", "Atenção!", MessageBoxButtons.OK, MessageBoxIcon.Information);

            LimparTela();
            textBoxNomeCliente.Focus();
        }

        private void dataGridViewPedidos_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (dataGridViewPedidos.DataSource == null)
                return;

            if (e.ColumnIndex == dataGridViewPedidos.Columns["Valor"].Index)
            {
                double valor = 0;

                if (!Double.TryParse(Convert.ToString(e.FormattedValue).Replace("R$","").Trim(), out valor))
                {
                    e.Cancel = true;
                    MessageBox.Show("Valor inválido!", "Atenção!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void FormImposto_Load(object sender, EventArgs e)
        {

        }
    }
}
