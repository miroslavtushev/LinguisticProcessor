   M S B A M L   `   `   `  ����B  >Program, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullC&clr-namespace:SEEL.LinguisticProcessorSEEL.LinguisticProcessor  5   	   :    4SEEL.LinguisticProcessor.WordExtractorMultipleWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   8local&clr-namespace:SEEL.LinguisticProcessor     5   	   -   5        $��WordExtractorW��5   	     � Title$*  $Word Extractor for Multiple Projects��6    $��314.886��6M   $��559.648��6^    � WindowState$ Normal=�6n    � 
ResizeMode$ CanMinimize=�6�   .��5	      �  � RowDefinitions 5
   
   �� 5      $��19 �6   �� 5      $��95 �6   �� 5      $	��140 �6   �� 5      $	��120 �6   .+�5   
   o� -   5         o�Name$ Tools_mn��5      $
��Left=�6!   $��19��6<   $	��Top=�6H   $��41��6`   $��0��6k   .@�5      m� $��_Tools��6   .@�5      m� -   5        $��_Word Extractor��5      64   m� 5      -   5        $��_Linguistic Change��5      67   m� 5      $��_Link Extractor��6   m� 5      $��_Project Downloader��6   m� 5      -   5        $��_Projects Statistics��5      69   �� 5      m� 5      $��_Exit��6   5      5      �� 5   
   -   5         ��Name$ Source��5      $��Multiple projects��6#   $
��Left=�6>   $��76��6Y   $��	10,10,0,0q�6e   $	��Top=�6x   $	��534��6�   $��1��6�   .��5      � .+�5      �� -   5         ��Name$ ProjectInputButton��5      $��	Browse...��65   $
��Left=�6I   $��	10,26,0,0q�6d   $	��Top=�6w   $��75��6�   $��19��6�   �� 5       -   5         ��Name$ FolderProjects��5       $
��Left=�62   $��20��6M   $��
111,25,0,0q�6Y    �� TextWrapping$
 Wrap=�6m   $��Select folder(s)...��6�   
�������$	��Top=�6�   $	��401��6�   	 �� 
IsReadOnly	 . {�.  6�   �� 5!      -	   5        
 ��Name$
 	textBlock��5!      $
��Left=�6/   $��10,5,0,0q�6J   $
��Wrap=�6\   $(��"Select the folders of the projects��6p   $	��Top=�6�   $	��480��6�   5"      5#      �� 5%   
   -
   5        $ StartWordExtraction_bt��5%      $��Start!��61   $
��Left=�6B   $��
345,79,0,0q�6]   $	��Top=�6q   $��75��6�   $��20��6�   $��2��6�   {�.  6�   �� 5&   
   -   5        $
 
textBlock1��5&      $
��Left=�6(   $��	233,7,0,0q�6C   $
��Wrap=�6V   $i��c2. Select the options you want to apply and hit Start!. By default, all options should be selected.��6j   $	��Top=�6�   $	��311��6�   $��2��6�   $��38��6  �� 5'   
   -   5        $ Code_options��5'      $��Code extraction options��6)   $
��Left=�6J   $	��106��6e   $��10,7,0,0q�6r   $��2��6�   $	��Top=�6�   $	��211��6�   .��5(      �  5)      �� 5*      $��26 �6$   �� 5+      $��26 �6$   �� 5,      $��26 �6$   .+�5.      �� -   5         ��Name$ IsCamelCaseBox��5.      $��camelCase splitting��63   $
��Left=�6Q   $��Center=�6l   $��10,6,0,5q�6�   $��0��6�   $	��160��6�   $��16��6�    d� 	IsChecked$
 TrueY�6�   6�   �� 5/      -   5        $ IsUnderscoreBox��5/      $��Underscore__splitting��64   $
��Left=�6T   $��Center=�6o   $��10,9,0,0q�6�   $��1��6�   $	��160��6�   $��16��6�   $
 TrueY�6�   6�   �� 50      -   5        $ IsCodeAsNaturalBox��50      $��Combine code and comments��67   $
��Left=�6[   $��Center=�6v   $��	10,10,0,1q�6�   $��2��6�   $	��179��6�   $��16��6�   $
 TrueY�6�   6�   51      52      �� 54   
   -   5        $��	StatusBar��54      $��Stretch=�6%   $��32��6C   $	��Top=�6O   $��3��6g   .@�55      � $	��544��6    � ColumnDefinitions 56      �� 57      $
��457* �6'   .+�59      �� -   5        $
 StatusBlock��59      $��10,-10,10,-9q�61   ��6G   �� *	Message+ �� UpdateSourceTrigger$ PropertyChanged=�5:      5;      5<      5=      