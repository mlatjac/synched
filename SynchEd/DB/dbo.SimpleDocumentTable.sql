﻿CREATE TABLE [dbo].[Document]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [XAMLContent] XML NULL
)


INSERT INTO [dbo].[Document]
           ([XAMLContent])
     VALUES
           (CAST(N'<Section xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation" xml:space="preserve" TextAlignment="Left" LineHeight="Auto" IsHyphenationEnabled="False" xml:lang="en-us" FlowDirection="LeftToRight" NumberSubstitution.CultureSource="Text" NumberSubstitution.Substitution="AsCulture" FontFamily="Segoe UI" FontStyle="Normal" FontWeight="Normal" FontStretch="Normal" FontSize="12" Foreground="#FF000000" Typography.StandardLigatures="True" Typography.ContextualLigatures="True" Typography.DiscretionaryLigatures="False" Typography.HistoricalLigatures="False" Typography.AnnotationAlternates="0" Typography.ContextualAlternates="True" Typography.HistoricalForms="False" Typography.Kerning="True" Typography.CapitalSpacing="False" Typography.CaseSensitiveForms="False" Typography.StylisticSet1="False" Typography.StylisticSet2="False" Typography.StylisticSet3="False" Typography.StylisticSet4="False" Typography.StylisticSet5="False" Typography.StylisticSet6="False" Typography.StylisticSet7="False" Typography.StylisticSet8="False" Typography.StylisticSet9="False" Typography.StylisticSet10="False" Typography.StylisticSet11="False" Typography.StylisticSet12="False" Typography.StylisticSet13="False" Typography.StylisticSet14="False" Typography.StylisticSet15="False" Typography.StylisticSet16="False" Typography.StylisticSet17="False" Typography.StylisticSet18="False" Typography.StylisticSet19="False" Typography.StylisticSet20="False" Typography.Fraction="Normal" Typography.SlashedZero="False" Typography.MathematicalGreek="False" Typography.EastAsianExpertForms="False" Typography.Variants="Normal" Typography.Capitals="Normal" Typography.NumeralStyle="Normal" Typography.NumeralAlignment="Normal" Typography.EastAsianWidths="Normal" Typography.EastAsianLanguage="Normal" Typography.StandardSwashes="0" Typography.ContextualSwashes="0" Typography.StylisticAlternates="0"><Paragraph><Run Foreground="#FFE81010">Some Text</Run></Paragraph><Paragraph><Run Foreground="#FFE81010">And new text too !</Run></Paragraph><Paragraph><Run>Some Text</Run><Run> </Run><Run> </Run></Paragraph></Section>' as XML))
GO

UPDATE [dbo].[Document] SET
XAMLContent = NULL where Id = 1
