namespace PROYECTO_TRANSPORTE_DELGADO_UCEDA_SAC
{
    partial class HISTORIAL_DEL_EXPEDIENTE
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle estiloEncabezado = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle estiloCeldas = new System.Windows.Forms.DataGridViewCellStyle();
            this.barraTitulo = new Guna.UI2.WinForms.Guna2Panel();
            this.lblTitulo = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.tablaDatos = new System.Windows.Forms.TableLayoutPanel();
            this.lblTextoCodigo = new System.Windows.Forms.Label();
            this.lblCodigo = new System.Windows.Forms.Label();
            this.lblTextoEstado = new System.Windows.Forms.Label();
            this.lblEstado = new System.Windows.Forms.Label();
            this.lblTextoSolicitante = new System.Windows.Forms.Label();
            this.lblSolicitante = new System.Windows.Forms.Label();
            this.lblTextoArea = new System.Windows.Forms.Label();
            this.lblArea = new System.Windows.Forms.Label();
            this.dgvHistorial = new System.Windows.Forms.DataGridView();
            this.colFecha = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHora = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAccion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colResponsable = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colObservacion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblTotal = new System.Windows.Forms.Label();
            this.btnVerDetalle = new Guna.UI2.WinForms.Guna2Button();
            this.btnCerrar = new Guna.UI2.WinForms.Guna2Button();
            this.tablaBotones = new System.Windows.Forms.TableLayoutPanel();
            this.panelFondo = new Guna.UI2.WinForms.Guna2Panel();
            this.barraTitulo.SuspendLayout();
            this.tablaDatos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistorial)).BeginInit();
            this.tablaBotones.SuspendLayout();
            this.panelFondo.SuspendLayout();
            this.SuspendLayout();
            //
            // lblTitulo
            //
            this.lblTitulo.BackColor = System.Drawing.Color.Transparent;
            this.lblTitulo.Font = new System.Drawing.Font("Cooper Black", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitulo.ForeColor = System.Drawing.Color.Black;
            this.lblTitulo.Location = new System.Drawing.Point(15, 9);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Historial del expediente";
            //
            // barraTitulo
            //
            this.barraTitulo.BackColor = System.Drawing.Color.Transparent;
            this.barraTitulo.BorderRadius = 8;
            this.barraTitulo.Controls.Add(this.lblTitulo);
            this.barraTitulo.FillColor = System.Drawing.Color.Gold;
            this.barraTitulo.Location = new System.Drawing.Point(15, 15);
            this.barraTitulo.Name = "barraTitulo";
            this.barraTitulo.Size = new System.Drawing.Size(840, 42);
            this.barraTitulo.TabIndex = 1;
            //
            // lblTextoCodigo
            //
            this.lblTextoCodigo.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblTextoCodigo.AutoSize = true;
            this.lblTextoCodigo.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTextoCodigo.ForeColor = System.Drawing.Color.Black;
            this.lblTextoCodigo.Name = "lblTextoCodigo";
            this.lblTextoCodigo.TabIndex = 2;
            this.lblTextoCodigo.Text = "Expediente:";
            //
            // lblCodigo
            //
            this.lblCodigo.AutoEllipsis = true;
            this.lblCodigo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCodigo.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCodigo.ForeColor = System.Drawing.Color.Black;
            this.lblCodigo.Name = "lblCodigo";
            this.lblCodigo.TabIndex = 3;
            this.lblCodigo.Text = "-";
            this.lblCodigo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // lblTextoEstado
            //
            this.lblTextoEstado.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblTextoEstado.AutoSize = true;
            this.lblTextoEstado.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTextoEstado.ForeColor = System.Drawing.Color.Black;
            this.lblTextoEstado.Name = "lblTextoEstado";
            this.lblTextoEstado.TabIndex = 4;
            this.lblTextoEstado.Text = "Estado:";
            //
            // lblEstado
            //
            this.lblEstado.AutoEllipsis = true;
            this.lblEstado.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblEstado.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEstado.ForeColor = System.Drawing.Color.Black;
            this.lblEstado.Name = "lblEstado";
            this.lblEstado.TabIndex = 5;
            this.lblEstado.Text = "-";
            this.lblEstado.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // lblTextoSolicitante
            //
            this.lblTextoSolicitante.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblTextoSolicitante.AutoSize = true;
            this.lblTextoSolicitante.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTextoSolicitante.ForeColor = System.Drawing.Color.Black;
            this.lblTextoSolicitante.Name = "lblTextoSolicitante";
            this.lblTextoSolicitante.TabIndex = 6;
            this.lblTextoSolicitante.Text = "Solicitante:";
            //
            // lblSolicitante
            //
            this.lblSolicitante.AutoEllipsis = true;
            this.lblSolicitante.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSolicitante.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSolicitante.ForeColor = System.Drawing.Color.Black;
            this.lblSolicitante.Name = "lblSolicitante";
            this.lblSolicitante.TabIndex = 7;
            this.lblSolicitante.Text = "-";
            this.lblSolicitante.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // lblTextoArea
            //
            this.lblTextoArea.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblTextoArea.AutoSize = true;
            this.lblTextoArea.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTextoArea.ForeColor = System.Drawing.Color.Black;
            this.lblTextoArea.Name = "lblTextoArea";
            this.lblTextoArea.TabIndex = 8;
            this.lblTextoArea.Text = "Área de destino:";
            //
            // lblArea
            //
            this.lblArea.AutoEllipsis = true;
            this.lblArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblArea.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblArea.ForeColor = System.Drawing.Color.Black;
            this.lblArea.Name = "lblArea";
            this.lblArea.TabIndex = 9;
            this.lblArea.Text = "-";
            this.lblArea.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // tablaDatos
            //
            this.tablaDatos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.tablaDatos.ColumnCount = 4;
            this.tablaDatos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16F));
            this.tablaDatos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34F));
            this.tablaDatos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18F));
            this.tablaDatos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32F));
            this.tablaDatos.Controls.Add(this.lblTextoCodigo, 0, 0);
            this.tablaDatos.Controls.Add(this.lblCodigo, 1, 0);
            this.tablaDatos.Controls.Add(this.lblTextoEstado, 2, 0);
            this.tablaDatos.Controls.Add(this.lblEstado, 3, 0);
            this.tablaDatos.Controls.Add(this.lblTextoSolicitante, 0, 1);
            this.tablaDatos.Controls.Add(this.lblSolicitante, 1, 1);
            this.tablaDatos.Controls.Add(this.lblTextoArea, 2, 1);
            this.tablaDatos.Controls.Add(this.lblArea, 3, 1);
            this.tablaDatos.Location = new System.Drawing.Point(15, 70);
            this.tablaDatos.Name = "tablaDatos";
            this.tablaDatos.Padding = new System.Windows.Forms.Padding(8, 4, 8, 4);
            this.tablaDatos.RowCount = 2;
            this.tablaDatos.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tablaDatos.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tablaDatos.Size = new System.Drawing.Size(840, 66);
            this.tablaDatos.TabIndex = 10;
            //
            // colFecha
            //
            this.colFecha.HeaderText = "Fecha";
            this.colFecha.Name = "colFecha";
            this.colFecha.ReadOnly = true;
            this.colFecha.Width = 95;
            //
            // colHora
            //
            this.colHora.HeaderText = "Hora";
            this.colHora.Name = "colHora";
            this.colHora.ReadOnly = true;
            this.colHora.Width = 95;
            //
            // colAccion
            //
            this.colAccion.HeaderText = "Acción";
            this.colAccion.Name = "colAccion";
            this.colAccion.ReadOnly = true;
            this.colAccion.Width = 175;
            //
            // colResponsable
            //
            this.colResponsable.HeaderText = "Responsable";
            this.colResponsable.Name = "colResponsable";
            this.colResponsable.ReadOnly = true;
            this.colResponsable.Width = 140;
            //
            // colObservacion
            //
            this.colObservacion.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colObservacion.HeaderText = "Observación";
            this.colObservacion.Name = "colObservacion";
            this.colObservacion.ReadOnly = true;
            //
            // dgvHistorial
            //
            this.dgvHistorial.AllowUserToAddRows = false;
            this.dgvHistorial.AllowUserToDeleteRows = false;
            this.dgvHistorial.AllowUserToResizeRows = false;
            this.dgvHistorial.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvHistorial.BackgroundColor = System.Drawing.Color.White;
            this.dgvHistorial.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            estiloEncabezado.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            estiloEncabezado.BackColor = System.Drawing.Color.Black;
            estiloEncabezado.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            estiloEncabezado.ForeColor = System.Drawing.Color.Gold;
            estiloEncabezado.SelectionBackColor = System.Drawing.Color.Black;
            estiloEncabezado.SelectionForeColor = System.Drawing.Color.Gold;
            estiloEncabezado.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvHistorial.ColumnHeadersDefaultCellStyle = estiloEncabezado;
            this.dgvHistorial.ColumnHeadersHeight = 34;
            this.dgvHistorial.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvHistorial.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colFecha,
            this.colHora,
            this.colAccion,
            this.colResponsable,
            this.colObservacion});
            estiloCeldas.BackColor = System.Drawing.Color.White;
            estiloCeldas.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            estiloCeldas.ForeColor = System.Drawing.Color.Black;
            estiloCeldas.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(236)))), ((int)(((byte)(140)))));
            estiloCeldas.SelectionForeColor = System.Drawing.Color.Black;
            estiloCeldas.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvHistorial.DefaultCellStyle = estiloCeldas;
            this.dgvHistorial.EnableHeadersVisualStyles = false;
            this.dgvHistorial.GridColor = System.Drawing.Color.Silver;
            this.dgvHistorial.Location = new System.Drawing.Point(15, 148);
            this.dgvHistorial.MultiSelect = false;
            this.dgvHistorial.Name = "dgvHistorial";
            this.dgvHistorial.ReadOnly = true;
            this.dgvHistorial.RowHeadersVisible = false;
            this.dgvHistorial.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvHistorial.Size = new System.Drawing.Size(840, 330);
            this.dgvHistorial.TabIndex = 11;
            //
            // lblTotal
            //
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotal.ForeColor = System.Drawing.Color.DimGray;
            this.lblTotal.Location = new System.Drawing.Point(15, 484);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.TabIndex = 12;
            this.lblTotal.Text = "0 acción(es) registradas";
            //
            // btnVerDetalle
            //
            this.btnVerDetalle.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnVerDetalle.BackColor = System.Drawing.Color.Transparent;
            this.btnVerDetalle.BorderRadius = 7;
            this.btnVerDetalle.FillColor = System.Drawing.Color.Gold;
            this.btnVerDetalle.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVerDetalle.ForeColor = System.Drawing.Color.Black;
            this.btnVerDetalle.Margin = new System.Windows.Forms.Padding(8, 3, 8, 3);
            this.btnVerDetalle.Name = "btnVerDetalle";
            this.btnVerDetalle.Size = new System.Drawing.Size(180, 38);
            this.btnVerDetalle.TabIndex = 13;
            this.btnVerDetalle.Text = "Ver detalle";
            this.btnVerDetalle.Click += new System.EventHandler(this.btnVerDetalle_Click);
            //
            // btnCerrar
            //
            this.btnCerrar.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnCerrar.BackColor = System.Drawing.Color.Transparent;
            this.btnCerrar.BorderRadius = 7;
            this.btnCerrar.FillColor = System.Drawing.Color.Black;
            this.btnCerrar.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCerrar.ForeColor = System.Drawing.Color.White;
            this.btnCerrar.Margin = new System.Windows.Forms.Padding(8, 3, 8, 3);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(180, 38);
            this.btnCerrar.TabIndex = 14;
            this.btnCerrar.Text = "Cerrar";
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            //
            // tablaBotones
            //
            this.tablaBotones.BackColor = System.Drawing.Color.White;
            this.tablaBotones.ColumnCount = 4;
            this.tablaBotones.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tablaBotones.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tablaBotones.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tablaBotones.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tablaBotones.Controls.Add(this.btnVerDetalle, 1, 0);
            this.tablaBotones.Controls.Add(this.btnCerrar, 2, 0);
            this.tablaBotones.Location = new System.Drawing.Point(15, 506);
            this.tablaBotones.Name = "tablaBotones";
            this.tablaBotones.RowCount = 1;
            this.tablaBotones.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tablaBotones.Size = new System.Drawing.Size(840, 48);
            this.tablaBotones.TabIndex = 15;
            //
            // panelFondo
            //
            this.panelFondo.BackColor = System.Drawing.Color.Transparent;
            this.panelFondo.BorderColor = System.Drawing.Color.Gold;
            this.panelFondo.BorderRadius = 10;
            this.panelFondo.BorderThickness = 2;
            this.panelFondo.Controls.Add(this.barraTitulo);
            this.panelFondo.Controls.Add(this.tablaDatos);
            this.panelFondo.Controls.Add(this.dgvHistorial);
            this.panelFondo.Controls.Add(this.lblTotal);
            this.panelFondo.Controls.Add(this.tablaBotones);
            this.panelFondo.FillColor = System.Drawing.Color.White;
            this.panelFondo.Location = new System.Drawing.Point(10, 10);
            this.panelFondo.Name = "panelFondo";
            this.panelFondo.Size = new System.Drawing.Size(870, 566);
            this.panelFondo.TabIndex = 16;
            //
            // HISTORIAL_DEL_EXPEDIENTE
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(890, 586);
            this.Controls.Add(this.panelFondo);
            this.Name = "HISTORIAL_DEL_EXPEDIENTE";
            this.Text = "Historial del expediente";
            this.barraTitulo.ResumeLayout(false);
            this.barraTitulo.PerformLayout();
            this.tablaDatos.ResumeLayout(false);
            this.tablaDatos.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistorial)).EndInit();
            this.tablaBotones.ResumeLayout(false);
            this.panelFondo.ResumeLayout(false);
            this.panelFondo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel barraTitulo;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblTitulo;
        private System.Windows.Forms.TableLayoutPanel tablaDatos;
        private System.Windows.Forms.Label lblTextoCodigo;
        private System.Windows.Forms.Label lblCodigo;
        private System.Windows.Forms.Label lblTextoEstado;
        private System.Windows.Forms.Label lblEstado;
        private System.Windows.Forms.Label lblTextoSolicitante;
        private System.Windows.Forms.Label lblSolicitante;
        private System.Windows.Forms.Label lblTextoArea;
        private System.Windows.Forms.Label lblArea;
        private System.Windows.Forms.DataGridView dgvHistorial;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFecha;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHora;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAccion;
        private System.Windows.Forms.DataGridViewTextBoxColumn colResponsable;
        private System.Windows.Forms.DataGridViewTextBoxColumn colObservacion;
        private System.Windows.Forms.Label lblTotal;
        private Guna.UI2.WinForms.Guna2Button btnVerDetalle;
        private Guna.UI2.WinForms.Guna2Button btnCerrar;
        private System.Windows.Forms.TableLayoutPanel tablaBotones;
        private Guna.UI2.WinForms.Guna2Panel panelFondo;
    }
}
