export type CompanyInvoice = {
    id: number
    clientRegNr: string;
    clientKMKR: string;
    title: string
    address: string
    city: string
    zipCode: number
    country: string
    invoiceNumber: number
    dateCreated: Date
    dateDue: Date
    condition: string
    delayFine: string
    font: string
    products: Product[]
    productsAndQuantities: Record<number, number>
    invoiceType: "company"
    selectedCompanyId: number
}
