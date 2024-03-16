# base image
FROM node:18.12

RUN mkdir /usr/src/app/
RUN mkdir /usr/src/app/frontend
WORKDIR /usr/src/app/frontend


COPY package.json ./
COPY package-lock.json ./
COPY tsconfig.json ./
COPY public ./public
COPY .env ./
COPY .env.development ./
COPY .env.production ./
COPY src ./src
# COPY tailwind.config.js ./

RUN npm install
# RUN npm install react-scripts tailwindcss --save-dev

#COPY . ./
#ARG REACT_APP_API_BASE_URL
#ENV REACT_APP_API_BASE_URL=$REACT_APP_API_BASE_URL
CMD ["npm", "start"]
EXPOSE 3000
