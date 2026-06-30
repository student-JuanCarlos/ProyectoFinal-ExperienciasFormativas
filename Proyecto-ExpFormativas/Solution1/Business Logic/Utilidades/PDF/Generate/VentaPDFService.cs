using Business_Logic.Utilidades.PDF.Interface;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Business_Logic.Utilidades.PDF.Generate
{
    public class VentaPDFService : IVentaPDF
    {

        public (byte[] Bytes, string Nombre) ObtenerArchivo(Venta v)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var dorado = "#C9A84C";
            var carbon = "#1A1A2E";
            var muted = "#8A8A9A";
            var borde = "#E8E8EC";

            byte[] pdf = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A5);
                    page.Margin(25);
                    page.DefaultTextStyle(t => t.FontSize(9).FontFamily(Fonts.Verdana).FontColor(carbon));

                    page.Header().Column(column =>
                    {
                        column.Item().AlignCenter().Text("LA ESTANCIA GOURMET")
                            .FontSize(16).Bold().FontColor(dorado);
                        column.Item().AlignCenter().PaddingTop(2)
                            .Text("Boleta de Venta").FontSize(9).FontColor(muted);
                        column.Item().PaddingTop(8).PaddingBottom(4)
                            .LineHorizontal(1.2f).LineColor(dorado);
                    });

                    page.Content().Column(column =>
                    {
                        column.Spacing(10);

                        column.Item().Column(seccion =>
                        {
                            seccion.Item().Text("INFORMACIÓN PRINCIPAL")
                                .FontSize(8).Bold().FontColor(dorado);
                            seccion.Item().PaddingTop(2).PaddingBottom(4)
                                .LineHorizontal(0.7f).LineColor(borde);

                            void Fila(string label, string valor)
                            {
                                seccion.Item().Row(row =>
                                {
                                    row.ConstantItem(90).Text(label).FontColor(muted);
                                    row.RelativeItem().Text(valor).SemiBold();
                                });
                            }

                            Fila("Cliente:", v.reserva.NombreCliente ?? v.reserva.cliente?.NombreCompleto ?? "-");
                            Fila("Tipo reserva:", v.reserva.TipoReserva);
                            Fila("Contacto:", v.reserva.TelefonoCliente ?? v.reserva.cliente?.Email ?? "-");
                            Fila("Personas:", v.reserva.CantidadPersonas.ToString());
                            Fila("Fecha venta:", v.FechaVenta.ToString("dd/MM/yyyy"));
                            Fila("Método pago:", v.MetodoPago);
                            Fila("Costo reserva:", $"S/ {v.reserva.CostoTotal:0.00}");
                        });

                        column.Item().Column(seccion =>
                        {
                            seccion.Item().Text("PLATILLOS")
                                .FontSize(8).Bold().FontColor(dorado);
                            seccion.Item().PaddingTop(2).PaddingBottom(4)
                                .LineHorizontal(0.7f).LineColor(borde);

                            seccion.Item().Table(table =>
                            {
                                table.ColumnsDefinition(c =>
                                {
                                    c.RelativeColumn(3);
                                    c.RelativeColumn(1);
                                    c.RelativeColumn(1.3f);
                                });

                                table.Header(h =>
                                {
                                    h.Cell().Text("Platillo").FontSize(8).Bold().FontColor(muted);
                                    h.Cell().AlignCenter().Text("Cant.").FontSize(8).Bold().FontColor(muted);
                                    h.Cell().AlignRight().Text("Subtotal").FontSize(8).Bold().FontColor(muted);
                                    h.Cell().ColumnSpan(3).PaddingTop(3).PaddingBottom(3)
                                        .LineHorizontal(0.5f).LineColor(borde);
                                });

                                foreach (var d in v.detalles ?? new List<DetalleVenta>())
                                {
                                    table.Cell().PaddingVertical(2).Text(d.platillo.NombrePlatillo);
                                    table.Cell().PaddingVertical(2).AlignCenter().Text(d.Cantidad.ToString());
                                    table.Cell().PaddingVertical(2).AlignRight()
                                        .Text($"S/ {(d.PrecioUnitario * d.Cantidad):0.00}");
                                }
                            });
                        });

                        if (v.descuentos != null && v.descuentos.Any())
                        {
                            column.Item().Column(seccion =>
                            {
                                seccion.Item().Text("DESCUENTOS")
                                    .FontSize(8).Bold().FontColor(dorado);
                                seccion.Item().PaddingTop(2).PaddingBottom(4)
                                    .LineHorizontal(0.7f).LineColor(borde);

                                foreach (var des in v.descuentos)
                                {
                                    seccion.Item().Row(row =>
                                    {
                                        row.RelativeItem().Text(des.descuento.NombreDescuento);
                                        row.ConstantItem(60).AlignRight()
                                            .Text($"-{des.DescuentoUnitario:0.00}%").FontColor("#DC2626");
                                    });
                                }
                            });
                        }

                        column.Item().PaddingTop(6).BorderTop(1).BorderColor(dorado)
                            .PaddingTop(8).Row(row =>
                            {
                                row.RelativeItem().Text("TOTAL").FontSize(12).Bold();
                                row.ConstantItem(100).AlignRight()
                                    .Text($"S/ {v.Total:0.00}").FontSize(14).Bold().FontColor(dorado);
                            });

                        column.Item().PaddingTop(4).AlignRight()
                            .Text($"Generado por: {v.usuario?.NombreUsuario ?? "-"}")
                            .FontSize(7.5f).FontColor(muted);
                    });

                    page.Footer().PaddingTop(10).Column(column =>
                    {
                        column.Item().LineHorizontal(0.7f).LineColor(borde);
                        column.Item().PaddingTop(6).AlignCenter()
                            .Text("Gracias por su preferencia")
                            .FontSize(8).Italic().FontColor(muted);
                    });
                });
            }).GeneratePdf();

            string nombre = $"Boleta de la Venta #{v.IdVenta}.pdf";
            return (pdf, nombre);
        }
    }
}
