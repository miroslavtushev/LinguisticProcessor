   M S B A M L   `   `   `  ���� B  >Program, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullC&clr-namespace:SEEL.LinguisticProcessorSEEL.LinguisticProcessor  7    1SEEL.LinguisticProcessor.ProjectsDownloaderWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     8local&clr-namespace:SEEL.LinguisticProcessor     -   $��ProjectDownloaderW��  � Title$  Project Downloader��$��301.661��$��559.648�� � WindowState$ Normal=� � 
ResizeMode$ CanMinimize=�.���  � RowDefinitions �� $��19 ��� $	��217 ��� $��42 �.+�o� -    o�Name$ Tools_mn��$
��Left=�$��19��$	��Top=�$��41��$��0��.@�m� $��_Tools��.@�m� -   $��_Word Extractor��m� -   $��_Linguistic Change��m� $��_Link Extractor��m� $��_Project Downloader��m� -   $��_Projects Statistics���� m� $��_Exit���� -   $��	StatusBar��$��Stretch=�$��37��$	��Top=�$��2��.@�� $	��544��$��34�� � ColumnDefinitions �� $
��457* �.+��� -    ��Name$ StatusBlock��$��5,0,0,0q����� *	Message+ �� UpdateSourceTrigger$ PropertyChanged=��� -    ��Name$ Source_Copy��$��Local statistics��$
��Left=�$	��100��$��	10,10,0,0q�$	��Top=�$	��534��$��1��.��� .+��� -	   $ 	textBlock��$
��Left=�$��	10,10,0,0q�$
��Wrap=�$6��0Select a text file with GitHub links to download��$	��Top=�$	��163���� -
   	 ��Name$	 TxtProjects��$
��Left=�$��22��$��	90,47,0,0q�
 �� TextWrapping$

 Wrap=�$��Select text file...��
�������$	��Top=�$	��272�� �� 
IsReadOnly . {�.  $��0���� -    ��Name$ ProjectInputButton��$��	Browse...��$
��Left=�$��	10,47,0,0q�$	��Top=�$��75��$��22��$��0���� -   $ StartButtonDownload��$��Start!��$
��Left=�$��
437,46,0,0q�$	��Top=�$��75��$��23��$��0���� -    ��Name$ LanguageSelector��$
��Left=�$��
367,47,0,0q�$	��Top=�$��69���� -   $ Source_Copy1��$��Extract Links��$
��Left=�$	��100��$��
10,112,0,0q�$	��Top=�$	��534��$��1��.��� .+��� -   $ StartButtonLinks��$��Start!��$
��Left=�$��
271,23,0,0q�$	��Top=�$��75��$��22��$��0���� -   $ LanguageSelectorLinks��$
��Left=�$��
197,23,0,0q�$	��Top=�$��69���� -   $	 sss��$
��Left=�$��	53,23,0,0q�$
��Wrap=�$��Specify # of links:��$	��Top=�$��22��$	��104���� -   $	 
NumOfLinks��$
��Left=�$��22��$��
157,23,0,0q�$

 Wrap=�$	��Top=�$��35��