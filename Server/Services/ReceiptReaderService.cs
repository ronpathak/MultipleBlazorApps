using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MultipleBlazorApps.Shared.Entities;
using MultipleBlazorApps.Shared.DTOs;
using Azure;
using Azure.AI.FormRecognizer.DocumentAnalysis;
using MultipleBlazorApps.Server.Migrations;


namespace MultipleBlazorApps.Server.Services
{

    public class ReceiptReaderService : IReceiptReaderService
    {

        string endpoint = "https://testingformrecognizer1.cognitiveservices.azure.com/";
        string apiKey = "872f8bd13ca44607941c58754ac5f57c";



        public async Task<Receipt> AnalyseFile(string FileURI)
        {
            Receipt receiptdetail = new Receipt();

            var credential = new AzureKeyCredential(apiKey);
            var client = new DocumentAnalysisClient(new Uri(endpoint), credential);


            //sample document
            //Uri receiptUri = new Uri("https://raw.githubusercontent.com/Azure/azure-sdk-for-python/main/sdk/formrecognizer/azure-ai-formrecognizer/tests/sample_forms/receipt/contoso-receipt.png");
            Uri receiptUri = new Uri(FileURI);
            AnalyzeDocumentOperation operation = await client.AnalyzeDocumentFromUriAsync(WaitUntil.Completed, "prebuilt-receipt", receiptUri);

            AnalyzeResult receipts = operation.Value;

            // To see the list of the supported fields returned by service and its corresponding types, consult:
            // https://aka.ms/formrecognizer/receiptfields

            foreach (AnalyzedDocument receipt in receipts.Documents)
            {
                if (receipt.Fields.TryGetValue("MerchantName", out DocumentField merchantNameField))
                {
                    if (merchantNameField.FieldType == DocumentFieldType.String)
                    {
                        receiptdetail.Vendor = merchantNameField.Value.AsString();
                        Console.WriteLine($"Merchant Name: '{receiptdetail.Vendor}', with confidence {merchantNameField.Confidence}");
                    }
                }

                if (receipt.Fields.TryGetValue("TransactionDate", out DocumentField transactionDateField))
                {
                    if (transactionDateField.FieldType == DocumentFieldType.Date)
                    {
                        DateTimeOffset transactionDate = transactionDateField.Value.AsDate();
                        receiptdetail.Date = transactionDate.DateTime;
                        Console.WriteLine($"Transaction Date: '{transactionDate}', with confidence {transactionDateField.Confidence}");
                    }
                }

                if (receipt.Fields.TryGetValue("Items", out DocumentField itemsField))
                {
                    if (itemsField.FieldType == DocumentFieldType.List)
                    {
                        foreach (DocumentField itemField in itemsField.Value.AsList())
                        {
                            Console.WriteLine("Item:");

                            if (itemField.FieldType == DocumentFieldType.Dictionary)
                            {
                                IReadOnlyDictionary<string, DocumentField> itemFields = itemField.Value.AsDictionary();

                                if (itemFields.TryGetValue("Description", out DocumentField itemDescriptionField))
                                {
                                    if (itemDescriptionField.FieldType == DocumentFieldType.String)
                                    {
                                        string itemDescription = itemDescriptionField.Value.AsString();

                                        Console.WriteLine($"  Description: '{itemDescription}', with confidence {itemDescriptionField.Confidence}");
                                    }
                                }

                                if (itemFields.TryGetValue("TotalPrice", out DocumentField itemTotalPriceField))
                                {
                                    if (itemTotalPriceField.FieldType == DocumentFieldType.Double)
                                    {
                                        double itemTotalPrice = itemTotalPriceField.Value.AsDouble();
                                        Console.WriteLine($"  Total Price: '{itemTotalPrice}', with confidence {itemTotalPriceField.Confidence}");
                                    }
                                }
                            }
                        }
                    }
                }

                if (receipt.Fields.TryGetValue("Total", out DocumentField totalField))
                {
                    if (totalField.FieldType == DocumentFieldType.Double)
                    {
                        double total = totalField.Value.AsDouble();
                        receiptdetail.Total = total;
                        Console.WriteLine($"Total: '{total}', with confidence '{totalField.Confidence}'");
                    }
                }

            }



            return receiptdetail;

        }

    }

}
