   M S B A M L   `   `   `  ���� B  >Program, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullC&clr-namespace:SEEL.LinguisticProcessorSEEL.LinguisticProcessor  =    7SEEL.LinguisticProcessor.LinguisticChangeMultipleWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     8local&clr-namespace:SEEL.LinguisticProcessor     -   $��LinguisticChangeW��  � Title$  Linguistic Change��$��454.22��$��559.648�� � WindowState$ Normal=� � 
ResizeMode$ CanMinimize=�.���  � RowDefinitions �� $��19 ��� $	��127 ��� $	��124 ��� $	��124 ��� $��95 �.+�o� -    o�Name$ Tools_mn��$
��Left=�$��19��$	��Top=�$��41��$��0��.@�m� $��_Tools��.@�m� -   $��_Word Extractor��m� -   $��_Linguistic Change��m� $��_Link Extractor��m� $��_Project Downloader��m� -   $��_Projects Statistics���� m� $��_Exit���� -    ��Name$ Source��$@��:Linguistic Change 1st and last releases: multiple projects��$
��Left=�$	��107��$��	10,10,0,0q�$	��Top=�$	��534��$��1��.��� .+��� -    ��Name$ FirstAndLastRelease_bt��$��	Browse...��$
��Left=�$��	10,26,0,0q�$	��Top=�$��75��$��19���� -    ��Name$ FolderProjects��$
��Left=�$��20��$��
111,25,0,0q� �� TextWrapping$
 Wrap=�$��Select folder(s)...��
�������$	��Top=�$	��401��	 �� 
IsReadOnly	 . {�.  �� -	   
 ��Name$
 	textBlock��$
��Left=�$��10,5,0,0q�$
��Wrap=�$(��"Select the folders of the projects��$	��Top=�$	��480���� -
   $( "SeveralProjectsStartCalculation_bt��$��
Calculate!��$
��Left=�$��
451,87,0,0q�$	��Top=�$��75��$��20��$��1��{�.  �� -   $��	StatusBar��$��Stretch=�$��24��$	��Top=�$��4��.@�� $	��544�� � ColumnDefinitions �� $	��69* ��� $
��457* �.+��� -   $
 StatusBlock��$��5,0,10,0q�$��1������ *Status+�� -   $
 Progress������ *
Progress+�� -   $ Source_Copy��$8��2Interquartile Linguistic Change: multiple projects��$
��Left=�$	��107��$��	10,10,0,0q�$	��Top=�$	��534��$��2��.��� .+��� -   $ Interquartile_bt��$��	Browse...��$
��Left=�$��	10,26,0,0q�$	��Top=�$��75��$��19���� -   $ FolderProjects1��$
��Left=�$��20��$��
111,25,0,0q�$
 Wrap=�$��Select folder(s)...��
�������$	��Top=�$	��401��	 . {�.  �� -   $
 
textBlock1��$
��Left=�$��10,5,0,0q�$
��Wrap=�$(��"Select the folders of the projects��$	��Top=�$	��480���� -    ��Name$ IsAverageBt��$��Calculate the averages��$
��Left=�$��	27,89,0,0q�$��2��$	��Top=�$��15��$	��139���� -   $&  InterquartileStartCalculation_bt��$��
Calculate!��$
��Left=�$��
451,86,0,0q�$	��Top=�$��75��$��20��$��2��{�.  �� -   $ Source_Copy1��$2��,Average Linguistic Change: multiple projects��$
��Left=�$	��107��$��	10,12,0,0q�$	��Top=�$	��534��$��3��.��� .+��� -   $ Survival_bt��$��	Browse...��$
��Left=�$��	10,26,0,0q�$	��Top=�$��75��$��19���� -   $ FolderProjects2��$
��Left=�$��20��$��
111,25,0,0q�$
 Wrap=�$��Select folder(s)...��
�������$	��Top=�$	��401��	 . {�.  �� -   $
 
textBlock2��$
��Left=�$��10,5,0,0q�$
��Wrap=�$(��"Select the folders of the projects��$	��Top=�$	��480��	� -    	� 	GroupName$ WordsOrLengthsSelector��$��By word types��$
��Left=�$��	10,60,0,0q�$	��Top=� d� 	IsChecked$
 TrueY�	� -   $ WordsOrLengthsSelector��$��By word lengths��$
��Left=�$��
111,60,0,0q�$	��Top=��� -   $! SurvivalStartCalculation_bt��$��
Calculate!��$
��Left=�$��
451,82,0,0q�$	��Top=�$��75��$��20��$��3��{�.  