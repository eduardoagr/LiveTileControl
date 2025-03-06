using System.Diagnostics;

namespace LiveTileControl.Controls;

public partial class LiveTileControl : ContentView {

    private bool _isFlipping;

    public LiveTileControl() {
        InitializeComponent();
        UpdateTitlePosition();
        UpdateTileSize();

        // Initial states
        FrontView.IsVisible = true;
        FrontView.RotationY = 0;

        BackView.IsVisible = false;
        BackView.RotationY = -90;

    }

    #region title properties

    public static readonly BindableProperty TitleProperty = BindableProperty.Create(
        nameof(Title),
        typeof(string),
        typeof(LiveTileControl));

    public string Title {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public static readonly BindableProperty TitleFontSizeProperty = BindableProperty.Create(
        nameof(TitleFontSize),
        typeof(int),
        typeof(LiveTileControl), 14);

    public int TitleFontSize {
        get => (int)GetValue(TitleFontSizeProperty);
        set => SetValue(TitleFontSizeProperty, value);
    }

    public static readonly BindableProperty TitleFontColorProperty = BindableProperty.Create(
        nameof(TitleFontColor),
        typeof(Color),
        typeof(LiveTileControl));

    public Color TitleFontColor {
        get => (Color)GetValue(TitleFontColorProperty);
        set => SetValue(TitleFontColorProperty, value);
    }

    public static readonly BindableProperty TitleFontFamilyProperty = BindableProperty.Create(
        nameof(TitleFontFamily),
        typeof(string),
        typeof(LiveTileControl),
        "Default");

    public string TitleFontFamily {
        get => (string)GetValue(TitleFontFamilyProperty);
        set => SetValue(TitleFontFamilyProperty, value);
    }

    public static readonly BindableProperty TitleFontAttributesProperty = BindableProperty.Create(
        nameof(TitleFontAttributes),
        typeof(FontAttributes),
        typeof(LiveTileControl));

    public FontAttributes TitleFontAttributes {
        get => (FontAttributes)GetValue(TitleFontAttributesProperty);
        set => SetValue(TitleFontAttributesProperty, value);
    }

    public static readonly BindableProperty TitlePositionProperty = BindableProperty.Create(
        nameof(TitlePosition),
        typeof(TitlePositions),
        typeof(LiveTileControl),
        TitlePositions.LowerLeft, propertyChanged: OnTitlePositionChanged);

    private static void OnTitlePositionChanged(BindableObject bindable, object oldValue, object newValue) {
        if(bindable is LiveTileControl control && newValue is TitlePositions) {
            control.UpdateTitlePosition();
        }
    }

    private void UpdateTitlePosition() {
        switch(TitlePosition) {
            case TitlePositions.UpperLeft:
                TitleLabel.HorizontalOptions = LayoutOptions.Start;
                TitleLabel.VerticalOptions = LayoutOptions.Start;
                break;
            case TitlePositions.UpperRight:
                TitleLabel.HorizontalOptions = LayoutOptions.End;
                TitleLabel.VerticalOptions = LayoutOptions.Start;
                break;
            case TitlePositions.LowerLeft:
                TitleLabel.HorizontalOptions = LayoutOptions.Start;
                TitleLabel.VerticalOptions = LayoutOptions.End;
                break;
            case TitlePositions.LowerRight:
                TitleLabel.HorizontalOptions = LayoutOptions.End;
                TitleLabel.VerticalOptions = LayoutOptions.End;
                break;
            case TitlePositions.Center:
                TitleLabel.HorizontalOptions = LayoutOptions.Center;
                TitleLabel.VerticalOptions = LayoutOptions.Center;
                break;
            default:
                Debug.WriteLine("Unknown TitlePosition");
                break;
        }
    }

    public TitlePositions TitlePosition {
        get => (TitlePositions)GetValue(TitlePositionProperty);
        set => SetValue(TitlePositionProperty, value);
    }

    public static readonly BindableProperty TitleMarginProperty = BindableProperty.Create(
        nameof(TitleMargin), typeof(Thickness),
        typeof(LiveTileControl),
        new Thickness(5, 0, 0, 5));

    public Thickness TitleMargin {
        get => (Thickness)GetValue(TitleMarginProperty);
        set => SetValue(TitleMarginProperty, value);
    }

    public enum TitlePositions {
        UpperLeft,
        UpperRight,
        LowerLeft,
        LowerRight,
        Center
    }

    #endregion

    #region contols properties

    public static readonly BindableProperty TileWidthProperty = BindableProperty.Create(
        nameof(TileWidth),
        typeof(int),
        typeof(LiveTileControl));

    public int TileWidth {
        get => (int)GetValue(TileWidthProperty);
        set => SetValue(TileWidthProperty, value);
    }

    public static readonly BindableProperty TileHeightProperty = BindableProperty.Create(
        nameof(TileHeight),
        typeof(int),
        typeof(LiveTileControl));

    public int TileHeight {
        get => (int)GetValue(TileHeightProperty);
        set => SetValue(TileHeightProperty, value);
    }

    public static readonly BindableProperty TileTransparencyProperty = BindableProperty.Create(
        nameof(TileTransparency),
        typeof(double),
        typeof(LiveTileControl),
        propertyChanged: OnTransparencyChanged);

    public double TileTransparency {
        get => (double)GetValue(TileTransparencyProperty);
        set => SetValue(TileTransparencyProperty, value);
    }

    public static readonly BindableProperty TileColorProperty = BindableProperty.Create(
        nameof(TileColor),
        typeof(Color),
        typeof(LiveTileControl));

    public Color TileColor {
        get => (Color)GetValue(TileColorProperty);
        set => SetValue(TileColorProperty, value);
    }

    public static readonly BindableProperty TileSizeProperty = BindableProperty.Create(
    nameof(TileSize),
    typeof(TileSizes),
    typeof(LiveTileControl),
    TileSizes.Medium, // Default to Medium
    propertyChanged: OnTileSizeChanged);

    public TileSizes TileSize {
        get => (TileSizes)GetValue(TileSizeProperty);
        set => SetValue(TileSizeProperty, value);
    }

    public static readonly BindableProperty DataMemberProperty = BindableProperty.Create(
    nameof(DataMember),
    typeof(string),
    typeof(LiveTileControl),
    "No Data",
    propertyChanged: OnDataMemberChanged);

    private static void OnDataMemberChanged(BindableObject bindable, object oldValue, object newValue) {
        if(bindable is LiveTileControl control) {
            Debug.WriteLine("Data changed to: " + newValue);
            control.FlipTile(); // Automatically flip the tile when DataMember changes
        }
    }

    public string DataMember {
        get => (string)GetValue(DataMemberProperty);
        set => SetValue(DataMemberProperty, value);
    }

    public static readonly BindableProperty FlipDurationProperty = BindableProperty.Create(
        nameof(FlipDuration),
        typeof(int),
        typeof(LiveTileControl),
        500);

    public int FlipDuration {
        get => (int)GetValue(FlipDurationProperty);
        set => SetValue(FlipDurationProperty, value);
    }


    private static void OnTileSizeChanged(BindableObject bindable, object oldValue, object newValue) {
        if(bindable is LiveTileControl control && newValue is TileSizes) {
            control.UpdateTileSize();
        }
    }

    private static void OnTransparencyChanged(BindableObject bindable, object oldValue, object newValue) {
        if(bindable is LiveTileControl control && newValue is double transparency) {
            control.UpdateBackgroundTransparency(transparency);
        }
    }

    private void UpdateBackgroundTransparency(double transparency) {
        float alpha = (float)Math.Clamp(transparency / 100.0, 0.0, 1.0);
        var baseColor = TileColor ?? Colors.Transparent;
        TileColor = baseColor.WithAlpha(alpha);
    }

    private void UpdateTileSize() {
        switch(TileSize) {
            case TileSizes.Small:
                TileWidth = 71;
                TileHeight = 71;
                IconFontSize = 38;
                TitleLabel.IsVisible = false;
                break;
            case TileSizes.Medium:
                TileWidth = 150;
                TileHeight = 150;
                IconFontSize = 72;
                TitleLabel.IsVisible = true;
                break;
            case TileSizes.Wide:
                TileWidth = 310;
                TileHeight = 150;
                IconFontSize = 110;
                TitleLabel.IsVisible = true;
                break;
            case TileSizes.Large:
                TileWidth = 310;
                TileHeight = 310;
                IconFontSize = 132;
                TitleLabel.IsVisible = true;
                break;
            default:
                Debug.WriteLine("Unknown TileSize");
                break;
        }
    }

    public enum TileSizes {
        Small,    // Corresponds to Small tile size
        Medium,   // Corresponds to Medium tile size
        Wide,     // Corresponds to Wide tile size
        Large     // Corresponds to Large tile size
    }

    #endregion

    #region Icon Properties

    public static readonly BindableProperty IconGlyphProperty = BindableProperty.Create(
        nameof(IconGlyph),
        typeof(string),
        typeof(LiveTileControl));

    public string IconGlyph {
        get => (string)GetValue(IconGlyphProperty);
        set => SetValue(IconGlyphProperty, value);
    }

    public static readonly BindableProperty IconFontFamilyProperty = BindableProperty.Create(
        nameof(IconFontFamily),
        typeof(string),
        typeof(LiveTileControl));

    public string IconFontFamily {
        get => (string)GetValue(IconFontFamilyProperty);
        set => SetValue(IconFontFamilyProperty, value);
    }

    public static readonly BindableProperty IconFontSizeProperty = BindableProperty.Create(
        nameof(IconFontSize),
        typeof(double),
        typeof(LiveTileControl),
        24.0); // Default size

    public double IconFontSize {
        get => (double)GetValue(IconFontSizeProperty);
        set => SetValue(IconFontSizeProperty, value);
    }

    public static readonly BindableProperty IconFontColorProperty = BindableProperty.Create(
        nameof(IconFontColor),
        typeof(Color),
        typeof(LiveTileControl),
        Colors.Black); // Default color

    public Color IconFontColor {
        get => (Color)GetValue(IconFontColorProperty);
        set => SetValue(IconFontColorProperty, value);
    }

    #endregion


    private void FlipTile() {
        var frontToBackAnimation = new Animation(
            (f) => RotationY = f,
            0, 90, Easing.Linear);

        var backToFrontAnimation = new Animation(
            (f) => RotationY = f,
            -90, 0, Easing.Linear);

        frontToBackAnimation.Commit(
            this,
            "FrontToBack",
            16,
            (uint)(FlipDuration / 2),
            Easing.Linear,
            (finished, canceled) => {
                // Visibility toggle logic
                FrontView.IsVisible = !FrontView.IsVisible;
                BackView.IsVisible = !BackView.IsVisible;
                // this.RotationY = -90; // Ensure back view rotation starts correctly

                // Commit back-to - front animation
                backToFrontAnimation.Commit(
                           this, "BackToFront", 16, (uint)(FlipDuration / 2), Easing.Linear, (finished, canceled) => {
                               //If you remove the following, the card will go back next time

                               FrontView.IsVisible = !FrontView.IsVisible;
                               BackView.IsVisible = !BackView.IsVisible;

                           });
            });
    }

    public async Task UpdateTileSafely(string newData) {
        if(_isFlipping) {
            return;
        }

        _isFlipping = true; // Lock flipping
        DataMember = newData; // Update the content displayed on the back of the tile
        FlipTile(); // Trigger the flip animation
        await Task.Delay(FlipDuration); // Wait for the animation to complete
        _isFlipping = false; // Unlock flipping
    }
}
