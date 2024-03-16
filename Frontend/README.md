# โครงสร้างไฟล์จะประกอบไปด้วย

1. public
2. src
   - assets 
      * ข้างใน folder จะประกอบด้วย font image และ scss หรือ css ต่างๆ
   - components
      * ข้างในจะประกอบด้วย component ที่ต้องใช้บ่อยๆ เช่น component card หรือ component button component จะมี index.js จะเป็นตัวรวม component ทั้งหมดไว้ แล้ว export component เมื่อ             ต้องการเรียกจะเรียกเฉพาะ index.js เราจะไม่เรียก component ตรง compoent จะมี compoent ที่ใช้ เฉพาะ compoent เราจะรวมไว้ในโฟรเดอร์ แล้ว expont index.js
   - config
      * จะเป็นที่เก็บค่า config ต่างๆไว้ เช่น endpoint ของ service เป็นต้น
   - router
      * จะเป็นตัวกำหนดเส้นทางการวิ่งไปหน้าต่างๆ ภายในเว็บ จะ route เป็น 2 ประเภท route ที่มีเงื่อนไขในการเข้าถึง และไม่ต้องมีเงื่อนไขในการเข้าถึง
   - templates
      * จะเป็นการเก็บโครงสร้างของหน้าเว็บ เช่น  Form input ที่สามารถใช้ตัวเดียวกันหลายหน้า
   - utilities
      * จะเป็นไฟล์เก็บฟังก์ชันที่ใช้งานบ่อยหรือฟังก์ชันที่ใช้งานร่วมกันหลายๆ ที่ เช่น ฟังก์ชัน isEmpty()
   - view
      * เอาไห้เก็บไฟล์ที่แสดงผลบนหน้าเว็ป

## Available Scripts

In the project directory, you can run:

### `npm start`

Runs the app in the development mode.\
Open [http://localhost:3000](http://localhost:3000) to view it in the browser.

The page will reload if you make edits.\
You will also see any lint errors in the console.

### `npm test`

Launches the test runner in the interactive watch mode.\
See the section about [running tests](https://facebook.github.io/create-react-app/docs/running-tests) for more information.

### `npm run build`

Builds the app for production to the `build` folder.\
It correctly bundles React in production mode and optimizes the build for the best performance.

The build is minified and the filenames include the hashes.\
Your app is ready to be deployed!

See the section about [deployment](https://facebook.github.io/create-react-app/docs/deployment) for more information.

### `npm run eject`

**Note: this is a one-way operation. Once you `eject`, you can’t go back!**

If you aren’t satisfied with the build tool and configuration choices, you can `eject` at any time. This command will remove the single build dependency from your project.

Instead, it will copy all the configuration files and the transitive dependencies (webpack, Babel, ESLint, etc) right into your project so you have full control over them. All of the commands except `eject` will still work, but they will point to the copied scripts so you can tweak them. At this point you’re on your own.

You don’t have to ever use `eject`. The curated feature set is suitable for small and middle deployments, and you shouldn’t feel obligated to use this feature. However we understand that this tool wouldn’t be useful if you couldn’t customize it when you are ready for it.

## Learn More

You can learn more in the [Create React App documentation](https://facebook.github.io/create-react-app/docs/getting-started).

To learn React, check out the [React documentation](https://reactjs.org/).

"homepage": "/intranet/",

