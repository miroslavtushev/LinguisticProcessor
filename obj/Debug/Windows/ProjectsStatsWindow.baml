   M S B A M L   `   `   `  ����B  >Program, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullC&clr-namespace:SEEL.LinguisticProcessorSEEL.LinguisticProcessor  5   	   2    ,SEEL.LinguisticProcessor.ProjectsStatsWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   8local&clr-namespace:SEEL.LinguisticProcessor     5   	   -   5        $��WordExtractorW��5   	     � Title$  Project Statistics��6    $��297.378��6;   $��559.648��6L    � WindowState$ Normal=�6\    � 
ResizeMode$ CanMinimize=�6q   .��5	      �  � RowDefinitions 5
   
   �� 5      $��19 �6   �� 5      $��95 �6   �� 5      $	��124 �6   �� 5      $��40 �6   .+�5   
   o� -   5         o�Name$ Tools_mn��5      $
��Left=�6!   $��19��6<   $	��Top=�6H   $��41��6`   $��0��6k   .@�5      m� $��_Tools��6   .@�5      m� -   5        $��_Word Extractor��5      64   m� 5      -   5        $��_Linguistic Change��5      67   m� 5      $��_Link Extractor��6   m� 5      $��_Project Downloader��6   m� 5      -   5        $��_Projects Statistics��5      69   �� 5      m� 5      $��_Exit��6   5      5      �� 5   
   -   5         ��Name$ Source��5      $��GitHub statistics��6#   $
��Left=�6>   $��76��6Y   $��	10,10,0,0q�6e   $	��Top=�6x   $	��534��6�   $��1��6�   .��5      � .+�5      �� -   5         ��Name$ Overwrite_cb��5      $ ��Overwrite existing dataset��61   $
��Left=�6V   $��
256,10,0,0q�6q   $	��Top=�6�    d� 	IsChecked$ FalseY�6�   6�   5       5!      �� 5#   
   -   5         ��Name$ StartCollectingStats_bt��5#      $��Start!��62   $
��Left=�6C   $��
455,42,0,0q�6^   $	��Top=�6r   $��75��6�   $��20��6�   $��1��6�   �� 5%   
   -	   5        $��	StatusBar��5%      $��Stretch=�6%   $��24��6C   $	��Top=�6O   $��4��6g   .@�5&      � $	��544��6   	 � ColumnDefinitions	 5'      �� 5(      $	��69* �6'   �� 5)      $
��457* �6'   .+�5+      �� -
   5        
 ��Name$
 StatusBlock��5+      $��5,0,10,0q�61   $��1��6�   ��6C   �� *	Message+ �� UpdateSourceTrigger$ PropertyChanged=�6�   �� 5,      -   5        $
 Progress��5,      ��6.   �� *
Progress+$ PropertyChanged=�5-      5.      �� 5/   
   -   5        $ Source_Copy��5/      $��Local statistics��6(   $
��Left=�6B   $	��100��6]   $��	10,10,0,0q�6j   $	��Top=�6}   $	��534��6�   $��2��6�   .��50      � .+�51      �� -   5        $
 	textBlock��51      $
��Left=�6/   $��	10,10,0,0q�6J   $
��Wrap=�6]   $8��2Calculates KLOC, # of releases, and their averages��6q   $	��Top=�6�   $	��163��6�   �� 52      -   5        $
 textBlock_Copy��52      $
��Left=�64   $��
10,-87,0,0q�6O   $
��Wrap=�6c   $:��4Collects statistics from GitHub (stars, forks, etc.)��6w   $	��Top=�6�   $	��163��6�   �� 53      -   5         ��Name$ FolderProjects��53      $
��Left=�62   $��19��6M   $��	90,47,0,0q�6Y    �� TextWrapping$
 Wrap=�6l   $��Select language folder...��6�   
�������$	��Top=�6�   $	��272��6�    �� 
IsReadOnly . {�.  $��0��6�   �� 54      -   5        $ ProjectInputButton��54      $��	Browse...��65   $
��Left=�6I   $��	10,47,0,0q�6d   $	��Top=�6w   $��75��6�   $��19��6�   $��0��6�   �� 55      -   5         ��Name$ LanguageSelector��55      $
��Left=�65   $��
367,47,0,0q�6P   $	��Top=�6d   $��69��6|   6�   56      57      �� 58   
   -   5        $" StartCollectingLocalStats_bt��58      $��Start!��67   $
��Left=�6H   $��
455,74,0,0q�6c   $	��Top=�6w   $��75��6�   $��20��6�   $��2��6�   {�.  6�   59      5:      