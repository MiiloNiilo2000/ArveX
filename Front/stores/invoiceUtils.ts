import axios from 'axios';

export async function generateInvoicePDF(state) {
  try {
    const response = await axios.post('http://localhost:5176/CreateInvoice', {
      title: state.title,
      address: state.address,
      zipCode: state.zipCode.toString(),
      country: state.country,
      invoiceNumber: parseInt(state.invoiceNumber),
      dateCreated: new Date(state.dateCreated).toISOString(),
      dateDue: new Date(state.dateDue).toISOString(),
      condition: state.condition || "",
      delayFine: state.delayFine || "",
      font: state.selectedFont
    }, { responseType: 'blob' });
    
    const url = window.URL.createObjectURL(new Blob([response.data]));
    const link = document.createElement('a');
    link.href = url;
    link.setAttribute('download', 'invoice.pdf');
    document.body.appendChild(link);
    link.click();
    link.remove();
  } catch (error) {
    console.error("Error generating PDF:", error);
  }
}