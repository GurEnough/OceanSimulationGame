namespace OceanLogic
{
    public class GameSettingsController
    {
        ISettingsController _menu;

        #region =====----- CTOR -----=====

        public GameSettingsController(ISettingsController menu)
        {
            _menu = menu;
        }

        #endregion

        #region =====----- PUBLIC METHODS -----=====

        public void Edit(DefaultSettings gameSettings)
        {
            foreach (var field in typeof(DefaultSettings).GetFields().Where(x => !x.IsStatic))
            {
                var val = field.GetValue(gameSettings);

                if (val is double)
                {
                    _menu.AddEntry(field.Name, (double)val, 0, 1);
                }

                if (val is int)
                {
                    _menu.AddEntry(field.Name, (int)val, 1, 500);
                }
            }

            _menu.MenuExit += (s, e) =>

            {
                foreach (var field in typeof(DefaultSettings).GetFields().Where(x => !x.IsStatic))
                {

                    double value = e.entered.Where((x) => x.entry == field.Name).First().val;

                    if (field.FieldType == typeof(int))
                    {
                        field.SetValue(gameSettings, (int)value);
                    }

                    else

                    {
                        field.SetValue(gameSettings, value);
                    }

                }

                if (SettingsEdited != null)

                {
                    SettingsEdited(this, new SettingsSaveEventArgs(gameSettings));
                }
            };

            _menu.Make();
        }

        #endregion


        public event SettingsSaveEventHandler SettingsEdited;
    }


    public class SettingsSaveEventArgs : EventArgs
    {
        public DefaultSettings settings;

        public SettingsSaveEventArgs(DefaultSettings mSettings)

        {
            settings = mSettings;
        }

    }

    public delegate void SettingsSaveEventHandler(object sender, SettingsSaveEventArgs e);

}
