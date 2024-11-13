export type Invoice = {
    invoiceId: number
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
}