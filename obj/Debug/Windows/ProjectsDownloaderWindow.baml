   M S B A M L   `   `   `  ����B  >Program, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullC&clr-namespace:SEEL.LinguisticProcessorSEEL.LinguisticProcessor  5   	   7    1SEEL.LinguisticProcessor.ProjectsDownloaderWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   8local&clr-namespace:SEEL.LinguisticProcessor     5   	   -   5        $��ProjectDownloaderW��5	   	     � Title$  Project Downloader��5   	   $��301.661��6$   $��559.648��65    � WindowState$ Normal=�6E    � 
ResizeMode$ CanMinimize=�6Z   .��5
      �  � RowDefinitions 5   
   �� 5      $��19 �6   �� 5      $	��217 �6   �� 5      $��42 �6   .+�5   
   o� -   5         o�Name$ Tools_mn��5      $
��Left=�6!   $��19��6<   $	��Top=�6H   $��41��6`   $��0��6k   .@�5      m� $��_Tools��6   .@�5      m� -   5        $��_Word Extractor��5      64   m� 5      -   5        $��_Linguistic Change��5      67   m� 5      $��_Link Extractor��6   m� 5      $��_Project Downloader��6   m� 5      -   5        $��_Projects Statistics��5      69   �� 5      m� 5      $��_Exit��6   5      5      �� 5   
   -   5        $��	StatusBar��5      $��Stretch=�6%   $��37��6C   $	��Top=�6O   $��2��6g   .@�5       � $	��544��6   $��34��6    � ColumnDefinitions 5!      �� 5"      $
��457* �6'   .+�5$      �� -   5         ��Name$ StatusBlock��5$      $��5,0,0,0q�61   ��6B   �� *	Message+ �� UpdateSourceTrigger$ PropertyChanged=�5%      5&      �� 5'   
   -   5         ��Name$ Source_Copy��5'      $��Local statistics��6(   $
��Left=�6B   $	��100��6]   $��	10,10,0,0q�6j   $	��Top=�6}   $	��534��6�   $��1��6�   .��5(      � .+�5)      �� -	   5        $ 	textBlock��5)      $
��Left=�6/   $��	10,10,0,0q�6J   $
��Wrap=�6]   $6��0Select a text file with GitHub links to download��6q   $	��Top=�6�   $	��163��6�   �� 5*      -
   5        	 ��Name$	 TxtProjects��5*      $
��Left=�6/   $��22��6J   $��	90,47,0,0q�6V   
 �� TextWrapping$

 Wrap=�6i   $��Select text file...��6}   
�������$	��Top=�6�   $	��272��6�    �� 
IsReadOnly . {�.  $��0��6�   �� 5+      -   5         ��Name$ ProjectInputButton��5+      $��	Browse...��65   $
��Left=�6I   $��	10,47,0,0q�6d   $	��Top=�6w   $��75��6�   $��22��6�   $��0��6�   �� 5,      -   5        $ StartButtonDownload��5,      $��Start!��66   $
��Left=�6G   $��
437,46,0,0q�6b   $	��Top=�6v   $��75��6�   $��23��6�   $��0��6�   �� 5-      -   5         ��Name$ LanguageSelector��5-      $
��Left=�65   $��
367,47,0,0q�6P   $	��Top=�6d   $��69��6|   6�   5.      5/      �� 50   
   -   5        $ Source_Copy1��50      $��Extract Links��6)   $
��Left=�6@   $	��100��6[   $��
10,112,0,0q�6h   $	��Top=�6|   $	��534��6�   $��1��6�   .��51      � .+�52      �� -   5        $ StartButtonLinks��52      $��Start!��63   $
��Left=�6D   $��
271,23,0,0q�6_   $	��Top=�6s   $��75��6�   $��22��6�   $��0��6�   �� 53      -   5        $ LanguageSelectorLinks��53      $
��Left=�6:   $��
197,23,0,0q�6U   $	��Top=�6i   $��69��6�   6�   �� 54      -   5        $	 sss��54      $
��Left=�6)   $��	53,23,0,0q�6D   $
��Wrap=�6W   $��Specify # of links:��6k   $	��Top=�6�   $��22��6�   $	��104��6�   �� 55      -   5        $	 
NumOfLinks��55      $
��Left=�6.   $��22��6I   $��
157,23,0,0q�6U   $

 Wrap=�6i   $	��Top=�6}   $��35��6�   56      57      58      59      