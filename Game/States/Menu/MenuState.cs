namespace Ainomis.Game.States.Menu {
  using Ainomis.Game.Components;
  using Ainomis.Game.Manager;
  using Ainomis.Game.Systems;
  using Ainomis.Shared.Camera;
  using Ainomis.Shared.Display;
  using Ainomis.Shared.Input;

  using Artemis;
  using Artemis.Manager;

  using Microsoft.Xna.Framework;
  using Microsoft.Xna.Framework.Content;
  using Microsoft.Xna.Framework.Graphics;
  using Microsoft.Xna.Framework.Media;

  using Common = Shared.Common;
  using GameAction = Game.Action;

  internal class MenuState : GameState, Common.IDrawable, Common.IUpdateable {
    // Private members
    private Camera2D _camera;
    private SpriteFont _backgroundFont;
    private Song _backgroundMusic;

    public MenuState(
      ContentManager content,
      GameStateManager gameStateManager,
      SpriteBatch spriteBatch,
      KeyActionBinder<GameAction> keyActionBinder,
      IDisplayInfo displayInfo) :
      base(content, gameStateManager, spriteBatch, keyActionBinder, displayInfo) {
    }

    public override void Enter() {
      _backgroundFont = Content.Load<SpriteFont>("Menu/Label");
      _backgroundMusic = Content.Load<Song>("Menu/Music");

      MediaPlayer.IsRepeating = true;
      MediaPlayer.Play(_backgroundMusic);

      this.SetupBackground(EntityWorld.CreateEntity()).Refresh();
    }

    public override void Exit() {
      MediaPlayer.IsRepeating = false;
      MediaPlayer.Stop();

      // Free our resources
      base.Exit();
    }

    public void Update(GameTime gameTime) {
      _camera.Update(gameTime);
      EntityWorld.Update();
    }

    public void Draw(GameTime gameTime) {
      SpriteBatch.Begin(_camera.Transform);
      EntityWorld.Draw();

      // Toggle the text at a regular interval
      if (gameTime.TotalGameTime.Seconds % 3 != 0) {
        this.DrawMenu();
      }

      SpriteBatch.End();
    }

    protected override void SetupSystems(SystemManager manager) {
      // The camera needs to be created first since some systems may use it
      _camera = new Camera2D(DisplayInfo.RelativeScale);

      manager.SetSystem(new BasicRenderSystem(SpriteBatch), GameLoopType.Draw);
      manager.SetSystem(new TranslationSystem(), GameLoopType.Update);
    }

    private void DrawMenu() {
      // TODO: Localize messages (use resx?)
      var message = "Press any button to start";
      var messageSize = _backgroundFont.MeasureString(message);
      var messagePosition = ((Vector2)DisplayInfo.VirtualResolution / 2f) - (messageSize / 2f);
      messagePosition.Y *= 0.8f;

      SpriteBatch.DrawString(_backgroundFont, message, messagePosition, Color.WhiteSmoke);
    }

    private Entity SetupBackground(Entity entity) {
      var texture = Content.Load<Texture2D>("Menu/Background");

      entity.AddComponent(new TextureComponent(texture));
      entity.AddComponent(new TransformComponent(0f, 0f));
      entity.AddComponent(new SpriteComponent() {
        // Stretch the image so it fits 1:1 to the screen area
        Scale = DisplayInfo.VirtualResolution / new Vector2(texture.Width, texture.Height),
      });

      return entity;
    }
  }
}