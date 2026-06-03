import express from 'express';
import scrapperRoutes from './src/routes/scrapperRoutes.js';

const app = express();
app.use(express.json());
const PORT = process.env.PORT || 4000;

app.use('/api', scrapperRoutes);

app.get('/', (req, res) => {
    res.send('Hello');
});

//   const url = 'https://www.scrapethissite.com/pages/simple/'; 

//   await page.goto(url);
//   const title = await page.title();

//   console.log(`Connected: ${title}`);

//   await browser.close();
// })();

app.listen(PORT, () => {
    console.log(`Server is running on port ${PORT}`);
});