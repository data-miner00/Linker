namespace Linker.Wpf
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
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
    using Linker.Core.Models;
    using Linker.Core.Repositories;

    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IArticleRepository articleRepository;
        private readonly IYoutubeRepository youtubeRepository;
        private readonly IWebsiteRepository websiteRepository;

        private IEnumerable<Article> cachedArticles;
        private IEnumerable<Website> cachedWebsites;
        private IEnumerable<Youtube> cachedYoutubes;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        /// <param name="articleRepository">The article repository.</param>
        /// <param name="youtubeRepository">The youtube repository.</param>
        /// <param name="websiteRepository">The website repository.</param>
        public MainWindow(
            IArticleRepository articleRepository,
            IYoutubeRepository youtubeRepository,
            IWebsiteRepository websiteRepository)
        {
            ArgumentNullException.ThrowIfNull(articleRepository);
            ArgumentNullException.ThrowIfNull(websiteRepository);
            ArgumentNullException.ThrowIfNull(youtubeRepository);

            this.InitializeComponent();

            this.articleRepository = articleRepository;
            this.youtubeRepository = youtubeRepository;
            this.websiteRepository = websiteRepository;

            this.cachedArticles = this.articleRepository.GetAllAsync(CancellationToken.None).GetAwaiter().GetResult();

            foreach (var article in this.cachedArticles)
            {
                this.sampleList.Items.Add(article.Title);
            }
        }

        private void btnLoadArticle_Click(object sender, RoutedEventArgs e)
        {
            if (this.cachedArticles is null || !this.cachedArticles.Any())
            {
                this.cachedArticles = this.articleRepository.GetAllAsync(default).GetAwaiter().GetResult();
            }

            this.sampleList.Items.Clear();
            this.PopulateListWithTitles(this.cachedArticles.Select(x => x.Title));
        }

        private void btnLoadWebsites_Click(object sender, RoutedEventArgs e)
        {
            if (this.cachedWebsites is null || !this.cachedWebsites.Any())
            {
                this.cachedWebsites = this.websiteRepository.GetAllAsync(default).GetAwaiter().GetResult();
            }

            this.sampleList.Items.Clear();
            this.PopulateListWithTitles(this.cachedWebsites.Select(x => x.Name));
        }

        private void btnLoadYoutube_Click(object sender, RoutedEventArgs e)
        {
            if (this.cachedYoutubes is null || !this.cachedYoutubes.Any())
            {
                this.cachedYoutubes = this.youtubeRepository.GetAllAsync(default).GetAwaiter().GetResult();
            }

            this.sampleList.Items.Clear();
            this.PopulateListWithTitles(this.cachedYoutubes.Select(x => x.Name));
        }

        private void PopulateListWithTitles(IEnumerable<string> titles)
        {
            foreach (var title in titles)
            {
                this.sampleList.Items.Add(title);
            }
        }
    }
}
