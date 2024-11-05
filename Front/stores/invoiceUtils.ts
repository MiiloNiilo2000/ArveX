import axios from 'axios';

export async function generateInvoicePDF(state) {
  try {
    console.log("Ids in util", state.productIds)
    const payload = {
      title: state.title,
      clientRegNr: state.clientRegNr.toString(),
      clientKMKR: state.clientKMKR,
      address: state.address,
      zipCode: state.zipCode.toString(),
      country: state.country,
      invoiceNumber: parseInt(state.invoiceNumber),
      dateCreated: new Date(state.dateCreated).toISOString(),
      dateDue: new Date(state.dateDue).toISOString(),
      condition: state.condition || "",
      delayFine: state.delayFine || "",
      font: state.selectedFont,
      productIds: state.productIds
    };

    const response = await axios.post('http://localhost:5176/CreateInvoice', payload, {
      headers: {
        'Content-Type': 'application/json'
      },
      responseType: 'blob'
    });

    const url = window.URL.createObjectURL(new Blob([response.data]));
    const link = document.createElement('a');
    link.href = url;
    link.setAttribute('download', 'invoice.pdf');
    document.body.appendChild(link);
    link.click();
    link.remove();
  } catch (error) {
    if (error.response) {
      console.error("Error response data:", error.response.data);
      console.error("Error response status:", error.response.status);
      console.error("Error response headers:", error.response.headers);
    } else {
      console.error("Error message:", error.message);
    }
  }
}
