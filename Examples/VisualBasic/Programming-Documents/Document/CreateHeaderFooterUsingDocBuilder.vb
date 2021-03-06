﻿Imports Microsoft.VisualBasic
Imports Aspose.Words
Imports Aspose.Words.Tables
Imports Aspose.Words.Drawing
Public Class CreateHeaderFooterUsingDocBuilder
    Public Shared Sub Run()
        ' ExStart:CreateHeaderFooterUsingDocBuilder
        ' The path to the documents directory.
        Dim dataDir As String = RunExamples.GetDataDir_WorkingWithDocument()

        Dim doc As New Document()
        Dim builder As New DocumentBuilder(doc)

        Dim currentSection As Section = builder.CurrentSection
        Dim pageSetup As PageSetup = currentSection.PageSetup

        ' Specify if we want headers/footers of the first page to be different from other pages.
        ' You can also use PageSetup.OddAndEvenPagesHeaderFooter property to specify
        ' different headers/footers for odd and even pages.
        pageSetup.DifferentFirstPageHeaderFooter = True

        ' --- Create header for the first page. ---
        pageSetup.HeaderDistance = 20
        builder.MoveToHeaderFooter(HeaderFooterType.HeaderFirst)
        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center

        ' Set font properties for header text.
        builder.Font.Name = "Arial"
        builder.Font.Bold = True
        builder.Font.Size = 14
        ' Specify header title for the first page.
        builder.Write("Aspose.Words Header/Footer Creation Primer - Title Page.")

        ' --- Create header for pages other than first. ---
        pageSetup.HeaderDistance = 20
        builder.MoveToHeaderFooter(HeaderFooterType.HeaderPrimary)

        ' Insert absolutely positioned image into the top/left corner of the header.
        ' Distance from the top/left edges of the page is set to 10 points.
        Dim imageFileName As String = dataDir & Convert.ToString("Aspose.Words.gif")
        builder.InsertImage(imageFileName, RelativeHorizontalPosition.Page, 10, RelativeVerticalPosition.Page, 10, 50, _
            50, WrapType.Through)

        builder.ParagraphFormat.Alignment = ParagraphAlignment.Right
        ' Specify another header title for other pages.
        builder.Write("Aspose.Words Header/Footer Creation Primer.")

        ' --- Create footer for pages other than first. ---
        builder.MoveToHeaderFooter(HeaderFooterType.FooterPrimary)

        ' We use table with two cells to make one part of the text on the line (with page numbering)
        ' to be aligned left, and the other part of the text (with copyright) to be aligned right.
        builder.StartTable()

        ' Clear table borders.
        builder.CellFormat.ClearFormatting()

        builder.InsertCell()

        ' Set first cell to 1/3 of the page width.
        builder.CellFormat.PreferredWidth = PreferredWidth.FromPercent(100 / 3)

        ' Insert page numbering text here.
        ' It uses PAGE and NUMPAGES fields to auto calculate current page number and total number of pages.
        builder.Write("Page ")
        builder.InsertField("PAGE", "")
        builder.Write(" of ")
        builder.InsertField("NUMPAGES", "")

        ' Align this text to the left.
        builder.CurrentParagraph.ParagraphFormat.Alignment = ParagraphAlignment.Left

        builder.InsertCell()
        ' Set the second cell to 2/3 of the page width.
        builder.CellFormat.PreferredWidth = PreferredWidth.FromPercent(100 * 2 / 3)

        builder.Write("(C) 2001 Aspose Pty Ltd. All rights reserved.")

        ' Align this text to the right.
        builder.CurrentParagraph.ParagraphFormat.Alignment = ParagraphAlignment.Right

        builder.EndRow()
        builder.EndTable()

        builder.MoveToDocumentEnd()
        ' Make page break to create a second page on which the primary headers/footers will be seen.
        builder.InsertBreak(BreakType.PageBreak)

        ' Make section break to create a third page with different page orientation.
        builder.InsertBreak(BreakType.SectionBreakNewPage)

        ' Get the new section and its page setup.
        currentSection = builder.CurrentSection
        pageSetup = currentSection.PageSetup

        ' Set page orientation of the new section to landscape.
        pageSetup.Orientation = Orientation.Landscape

        ' This section does not need different first page header/footer.
        ' We need only one title page in the document and the header/footer for this page
        ' has already been defined in the previous section
        pageSetup.DifferentFirstPageHeaderFooter = False

        ' This section displays headers/footers from the previous section by default.
        ' Call currentSection.HeadersFooters.LinkToPrevious(false) to cancel this.
        ' Page width is different for the new section and therefore we need to set 
        ' a different cell widths for a footer table.
        currentSection.HeadersFooters.LinkToPrevious(False)

        ' If we want to use the already existing header/footer set for this section 
        ' but with some minor modifications then it may be expedient to copy headers/footers
        ' from the previous section and apply the necessary modifications where we want them.
        CopyHeadersFootersFromPreviousSection(currentSection)

        ' Find the footer that we want to change.
        Dim primaryFooter As HeaderFooter = currentSection.HeadersFooters(HeaderFooterType.FooterPrimary)

        Dim row As Row = primaryFooter.Tables(0).FirstRow
        row.FirstCell.CellFormat.PreferredWidth = PreferredWidth.FromPercent(100 / 3)
        row.LastCell.CellFormat.PreferredWidth = PreferredWidth.FromPercent(100 * 2 / 3)



        dataDir = dataDir & Convert.ToString("HeaderFooter.Primer_out_.doc")
        ' Save the resulting document.
        doc.Save(dataDir)
        ' ExEnd:CreateHeaderFooterUsingDocBuilder

        Console.WriteLine(Convert.ToString(vbLf & "Header and footer created successfully using document builder." & vbLf & "File saved at ") & dataDir)
    End Sub
    ' ExStart:CopyHeadersFootersFromPreviousSection
    ''' <summary>
    ''' Clones and copies headers/footers form the previous section to the specified section.
    ''' </summary>
    Private Shared Sub CopyHeadersFootersFromPreviousSection(section As Section)
        Dim previousSection As Section = DirectCast(section.PreviousSibling, Section)

        If previousSection Is Nothing Then
            Return
        End If

        section.HeadersFooters.Clear()

        For Each headerFooter As HeaderFooter In previousSection.HeadersFooters
            section.HeadersFooters.Add(headerFooter.Clone(True))
        Next
    End Sub
    ' ExEnd:CopyHeadersFootersFromPreviousSection
End Class
