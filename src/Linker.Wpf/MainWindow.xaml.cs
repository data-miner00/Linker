namespace Linker.Wpf
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Navigation;
    using System.Windows.Shapes;
    using Linker.Core.Repositories;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IArticleRepository articleRepository;

        public MainWindow(IArticleRepository articleRepository)
        {
            this.InitializeComponent();

            this.articleRepository = articleRepository;

            var articles = this.articleRepository.GetAllAsync().GetAwaiter().GetResult();

            foreach (var article in articles)
            {
                this.sampleList.Items.Add(article.Title);
            }
        }
    }
}
