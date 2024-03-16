import React from 'react';
import './App.css';
import CssBaseline from '@mui/material/CssBaseline';
import { ThemeProvider } from '@mui/material';
import { useTranslation } from 'react-i18next';
import route from 'router';
import { Route, Routes, useParams } from 'react-router-dom';
import { Provider } from 'react-redux';
import theme from './theme/themes';
import storeRedux from 'store';
import Template from 'components/DialogAlert/STCustom/STCustomTemplate';
// import Template from 'components/DialogAlert/SnackBar/SnackBarTemplate';
// import Template from 'components/DialogAlert/SweetAlert/SweetAlertTemplate';
import { I18n } from 'utilities/utilities';
import { Language } from 'enum/enum';
import BlockUI from 'components/BlockUI/BlockUI';


const store = storeRedux();
I18n.InitialI18next();
const App: React.FC = () => {
  const { i18n } = useTranslation();
  const handleChangeLanguage = () => {
    //i18next.addResourceBundle('th', 'translations', , true, true);
    I18n.SetLanguage(Language.th)
    i18n.changeLanguage("th", (err, t) => {
      if (err) return console.log('something went wrong loading', err);
      t('key'); // -> same as i18next.t
    });
  }
  const handleChangeLanguageEN = () => {
    I18n.SetLanguage(Language.en)
    i18n.changeLanguage("en", (err, t) => {
      if (err) return console.log('something went wrong loading', err);
      t('key'); // -> same as i18next.t
    });
  }

  return (
    <Provider store={store}>
      <ThemeProvider theme={theme}>
        <CssBaseline />
        <Template />
        {/* <button onClick={() => { handleChangeLanguage() }}>TH</button>
        <button onClick={() => { handleChangeLanguageEN() }}>EN</button> */}
        <Routes>
          {route.map((o) => {
            return (
              <Route
                path={o.path}
                key={o.path}
                element={
                  o.layout ? (<o.layout><o.component /></o.layout>) : (<o.component />)
                }
              />
            );
          })}
        </Routes>
        <BlockUI />
      </ThemeProvider>
    </Provider>
  );
};

export default App;

