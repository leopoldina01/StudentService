   M S B A M L   `   `   `  ����C  ?Projekat, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:WpfApp1.ViewWpfApp1.View  5   	   =#clr-namespace:Projekat.LocalizationProjekat.Localization  5   	        WpfApp1.View.PredmetChange   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ,localclr-namespace:WpfApp1.View     5   	   2ns#clr-namespace:Projekat.Localization     5   	   $	��500��5	   (   $	��800��65     � Title  6	   (   "Projekat.Localization.LocExtension  *PredmetChange+ � 	Resources 5
         ValidationTemplate ��Name��%�   &	        �� 5   
   .R�5      �� .+�5      �� 
���  ��$��1,1,1,1q�6+   ._�5      �� $ textBox��60   5      �� 5      
���  ����6   �� *[0].ErrorContent+6>   5      5      5      .��5      �  � RowDefinitions 5   
   �� 5      $	��95* �6   �� 5      $	��26* �6    � ColumnDefinitions 5   
   �� 5      �� 5      .+�5   
   �� $��0��6   $��0��6"   $��0,15,0,8q�62   .+�5       �� $
��Wrap=�6   $��18(�6B    �� TextAlignment$ Center=�6P   $��	0,10,0,15q�6g   ��6,     *Šifra+6g   �� 5!      $
��Wrap=�6   $��18(�6B   $ Center=�6P   $��	0,10,0,15q�6g   ��6,     *Naziv+6g   �� 5"      $
��Wrap=�6   $��18(�6E   $ Center=�6S   $��	0,10,0,15q�6j   ��6,     *
Semestar+6j   �� 5#      $
��Wrap=�6   $��18(�6K   $ Center=�6Y   $��	0,10,0,15q�6p   ��6,     *Godina studija+6p   �� 5$      $
��Wrap=�6   $
��ESPB��6,   $��18(�68   $ Center=�6F   $��	0,10,0,15q�6]   �� 5%      $
��Wrap=�6   $��18(�6E   $ Center=�6S   $��	0,10,0,15q�6j   ��6,     *
Profesor+6j   5&      �� 5(   
   $��0��6   $��1��6"   $��0,15,0,4q�62   .+�5)      �� -   5         �� TextWrapping$
 Wrap=�5)       =� ErrorTemplate# [  6�   $	��270��6�   $��	0,10,0,15q�6�   $��24��6�   $
��Left=�6  ��6*   ��  �� Path$ Sifra�	 �� UpdateSourceTrigger$	 PropertyChanged=�
 �� ValidatesOnDataErrors$

 True��6  �� 5*      -   5        $
 Wrap=�5*      # [  6�   $	��270��6�   $��	0,10,0,15q�6�   $��24��6�   $
��Left=�6  ��6*   �� $ Naziv�$	 PropertyChanged=�$

 True��6  �� 5+      -   5        $��StatusComboBox��5+       �� SelectedValuePath$ Content��6-   $	��270��6�   $��	0,10,0,15q�6�   $��23.94��6�   $
��Left=�6   �� SelectedValue 6I   �� $ Semestar�$	 PropertyChanged=�$

 True��.@�5,      �� $��Zimski��6   �� 5-      $��Letnji��6   5.      �� 5/      -   5        $��GodinaStudijaComboBox��5/      $ Content��64   $	��270��6�   $��	0,10,0,15q�6�   $��23.94��6�   $
��Left=�6   6P   �� $ GodinaStudija�$	 PropertyChanged=�$

 True��.@�50      �� $
��Prva��6   �� 51      $��Druga��6   �� 52      $��Treća��6   �� 53      $��Četvrta��6   54      �� 55      -   5        $
 Wrap=�55      # [  6�   $	��270��6�   $��	0,10,0,15q�6�   $��23.94��6�   $
��Left=�6  ��6*   �� $
 Espb�$	 PropertyChanged=�$

 True��6  �� 56       �� Orientation$ 
Horizontal=�6   $��	0,10,0,15q�62   .+�57      �� -   5         ��Name$! TextBox_ImePrezimeProfesora��57      $
 Wrap=�6?   # [  6�   $	��190��6�   $��25��6  $
��Left=�6:  ��6S   �� *ImePrezimeProfesora+$	 PropertyChanged=�$

 True��6:  �� 58      -   5         ��Name$ ButtonDodajProfesora��58      $�� ��67   $��25��6B   $��30��6N   $��10,0,0,0q�6|   ��59      �� 5:       �� ImageSource$ /Assets/new_icon.png��6%   5<      �� 5=      -   5        $ ButtonUkloniProfesora��5=      $�� ��68   $��25��6C   $��30��6O   $��10,0,0,0q�6~   ��5>      �� 5?      $ /Assets/remove_icon.png��6%   5A      5B      5C      �� 5D   
   -	   5        $��ButtonPotvrdi��5D      $��1��6A   $��0��6N   $��150,30,150,30q�6{   ��6&     *	Potvrdi+6{   �� 5E   
   -
   5        $��ButtonOdustani��5E      $��1��6C   $��1��6P   $��150,30,150,30q�6~   ��6'     *
Odustani+6~   5F      5G      